using FlagshipProductTest.Shared;
using FlagshipProductTest.Shared.DAOs;
using FlagshipProductTest.Shared.DTOs;
using FlagshipProductTest.Shared.DTOs.User;
using FlagshipProductTest.Shared.Models;
using FlagshipProductTest.Shared.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlagshipProductTest.Core.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserDAO _userDAO;
        private readonly IDocumentDAO _documentDAO;
        private readonly IUserContributionDAO _userContributiontDAO;
        private readonly IValidators _validators;
        private readonly IMessageProvider _messageProvider;
        private readonly ICryptoProvider _cryptoProvider;
        private readonly SystemSettings _settings;
        public UserService(ILogger<UserService> logger, IUserDAO userDAO, IDocumentDAO documentDAO, IValidators validators, IMessageProvider messageProvider,
                           ICryptoProvider cryptoProvider, IOptions<SystemSettings> settings, IUserContributionDAO userContributiontDAO)
        {
            _logger = logger;
            _userDAO = userDAO;
            _documentDAO = documentDAO;
            _userContributiontDAO = userContributiontDAO;
            _validators = validators;
            _messageProvider = messageProvider;
            _cryptoProvider = cryptoProvider;
            _settings = settings.Value;
        }

        public async Task<BasicResponse> AddUserContributionAsync(AddContributionRequestDTO request)
        {
            var response = new BasicResponse(false);
            var user = await _userDAO.FindByUsername(request.Username);

            var contribution = new UserContribution
            {
                Amount = request.Amount,
                ContributionType = request.ContributionType,
                UserId = user.Id
            };

            _ = await _userContributiontDAO.Add(contribution);
            response.IsSuccessful = true;
            return response;
        }

        public async Task<PayloadResponse<UserLoginResponseDTO>> AuthenticatUserAsync(UserLoginRequestDTO request)
        {
            var response = new PayloadResponse<UserLoginResponseDTO>(false);
            var user = await _userDAO.FindByUsername(request.Username);
            if (user == null)
            {
                return ErrorResponse.Create<PayloadResponse<UserLoginResponseDTO>>(FaultMode.REQUESTED_ENTITY_NOT_FOUND, ResponseCodes.INVALID_LOGIN_CREDENTIALS,
                       _messageProvider.GetMessage(ResponseCodes.INVALID_LOGIN_CREDENTIALS));
            }
            if (user.IsLocked(_settings.MaxLoginCount))
            {
                return ErrorResponse.Create<PayloadResponse<UserLoginResponseDTO>>(
                      FaultMode.CLIENT_INVALID_ARGUMENT,
                      ResponseCodes.ACCOUNT_LOCKED,
                      $"{_messageProvider.GetMessage(ResponseCodes.ACCOUNT_LOCKED)}");

            }
            if (user.ProfileStatus == ProfileStatus.Inactive)
            {
                return ErrorResponse.Create<PayloadResponse<UserLoginResponseDTO>>(
                      FaultMode.CLIENT_INVALID_ARGUMENT,
                      ResponseCodes.ACCOUNT_DEACTIVATED,
                      $"{_messageProvider.GetMessage(ResponseCodes.ACCOUNT_DEACTIVATED)}");
            }

            var isPasswordEqual = await _cryptoProvider.AreEqualAsync(request.Password, user.Password, user.PasswordSalt);
            if (!isPasswordEqual)
            {
                user.FailedLoginCount++;
                await _userDAO.Update(user);
                return ErrorResponse.Create<PayloadResponse<UserLoginResponseDTO>>(FaultMode.REQUESTED_ENTITY_NOT_FOUND, ResponseCodes.INVALID_LOGIN_CREDENTIALS,
                       _messageProvider.GetMessage(ResponseCodes.INVALID_LOGIN_CREDENTIALS));
            }

            user.FailedLoginCount = 0;
            user.LastLogin = DateTime.Now;
            var jwtToken = await _cryptoProvider.GenerateJwtToken(user);
            await _userDAO.Update(user);

            response.SetPayload(new UserLoginResponseDTO
            {
                Access_Token = jwtToken.Access_Token,
                ExpirationTime = jwtToken.ExpirationTime,
                UserDetails = new UserDTO
                {
                    Username = user.Username,
                    EmailAddress = user.EmailAddress,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    LastLogin = user.LastLogin.Value,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender.ToString(),
                    Title = user.Title.ToString()
                }
            });
            response.IsSuccessful = true;
            return response;
        }

        public async Task<PayloadResponse<GetUserContributionResponseDTO>> GetUserContributionDetailsAsync(string username)
        {
            var response = new PayloadResponse<GetUserContributionResponseDTO>(false);
            var user = await _userDAO.FindByUsername(username);
            var userContributions = await _userContributiontDAO.FindByUserId(user.Id);
            if (!userContributions.Any())
            {
                return ErrorResponse.Create<PayloadResponse<GetUserContributionResponseDTO>>(FaultMode.REQUESTED_ENTITY_NOT_FOUND, ResponseCodes.NO_CONTRIBUTION,
                       _messageProvider.GetMessage(ResponseCodes.NO_CONTRIBUTION));
            }

            var payload = new GetUserContributionResponseDTO
            {
                TotalContributionAmount = userContributions.Sum(x => x.Amount),
                UserContributionDetails = userContributions.GroupBy(x => x.ContributionType).Select(x => new UserContributionPerTypeDTO
                {
                    TotalTypeContributionAmount = x.Sum(x => x.Amount),
                    ContributionType = x.FirstOrDefault().ContributionType.GetEnumDescription(),
                    UserContributions = x.Select(x => new UserContributionDTO
                    {
                        Amount = x.Amount,
                        DateCreated = x.DateCreated,
                        ContributionType = x.ContributionType.GetEnumDescription()
                    }).ToList()
                }).ToList()
            };

            response.SetPayload(payload);
            response.IsSuccessful = true;
            return response;
        }

        public async Task<BasicResponse> OnboardUserAsync(UserOnboardingRequestDTO request)
        {
            var response = new BasicResponse(false);
            var isValidUsername = await _validators.IsUsernameValidAsync(request.Username);
            if (!isValidUsername)
            {
                return ErrorResponse.Create<BasicResponse>(FaultMode.INVALID_OBJECT_STATE, ResponseCodes.INVALID_USERNAME_FORMAT,
                       _messageProvider.GetMessage(ResponseCodes.INVALID_USERNAME_FORMAT));
            }
            var usernameIsAvailable = await _userDAO.UsernameIsAvailable(request.Username);
            if (!usernameIsAvailable)
            {
                return ErrorResponse.Create<BasicResponse>(FaultMode.INVALID_OBJECT_STATE,
                        ResponseCodes.USERNAME_ALREADY_TAKEN, _messageProvider.GetMessage(ResponseCodes.USERNAME_ALREADY_TAKEN));
            };
            var dobIsInvalid = await _validators.IsValidDobAsync(request.DateOfBirth);
            if (dobIsInvalid)
            {
                return ErrorResponse.Create<BasicResponse>(FaultMode.INVALID_OBJECT_STATE,
                        ResponseCodes.INVALID_DOB, _messageProvider.GetMessage(ResponseCodes.INVALID_DOB));
            };
            var isValidBase64 = Util.IsBase64String(request.Passport.RawData);
            if (!isValidBase64)
            {
                return ErrorResponse.Create<BasicResponse>(FaultMode.INVALID_OBJECT_STATE,
                        ResponseCodes.NOT_BASE64, _messageProvider.GetMessage(ResponseCodes.NOT_BASE64));
            };
            var passwordSalt = await _cryptoProvider.GenerateSaltAsync();
            var passwordHash = await _cryptoProvider.GenerateHashAsync(request.NewPassword.Password, passwordSalt);

            var ticket = _userDAO.Begin();
            _documentDAO.Join(ticket);
            try
            {
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PasswordSalt = passwordSalt,
                    Password = passwordHash,
                    DateOfBirth = request.DateOfBirth,
                    EmailAddress = request.EmailAddress,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Username = request.Username,
                    Gender = request.Gender,
                    Title = request.Title,
                    FailedLoginCount = 0,
                    ProfileStatus = ProfileStatus.Active
                };
                user.Id = await _userDAO.Add(user);

                var userPassport = new Document
                {
                    UserId = user.Id,
                    DocumentType = DocumentType.Passport,
                    Extension = request.Passport.Extension,
                    RawData = request.Passport.RawData
                };
                _ = await _documentDAO.Add(userPassport);

                ticket.Commit();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating user {request.Username}");
                ticket.Rollback();
                return response;
            }
            response.IsSuccessful = true;
            return response;
        }
    }
}
