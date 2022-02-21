using FlagshipProductTest.Shared;
using FlagshipProductTest.Shared.DAOs;
using FlagshipProductTest.Shared.DTOs;
using FlagshipProductTest.Shared.DTOs.Admin;
using FlagshipProductTest.Shared.DTOs.User;
using FlagshipProductTest.Shared.Models;
using FlagshipProductTest.Shared.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Core.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ILogger<AdminService> _logger;
        private readonly IUserDAO _userDAO;
        private readonly IAdminUserDAO _adminUserDAO;
        private readonly IUserContributionDAO _userContributiontDAO;
        private readonly IValidators _validators;
        private readonly IMessageProvider _messageProvider;
        private readonly ICryptoProvider _cryptoProvider;
        private readonly SystemSettings _settings;
        public AdminService(ILogger<AdminService> logger, IUserDAO userDAO, IAdminUserDAO adminUserDAO, IValidators validators, IMessageProvider messageProvider,
                            ICryptoProvider cryptoProvider, IOptions<SystemSettings> settings, IUserContributionDAO userContributiontDAO)
        {
            _logger = logger;
            _userDAO = userDAO;
            _adminUserDAO = adminUserDAO;
            _userContributiontDAO = userContributiontDAO;
            _validators = validators;
            _messageProvider = messageProvider;
            _cryptoProvider = cryptoProvider;
            _settings = settings.Value;
        }

        public async Task<PayloadResponse<AdminLoginResponseDTO>> AuthenticatAdminUserAsync(AdminLoginRequestDTO request)
        {
            var response = new PayloadResponse<AdminLoginResponseDTO>(false);
            var user = await _adminUserDAO.FindByUsername(request.Username);
            if (user == null)
            {
                return ErrorResponse.Create<PayloadResponse<AdminLoginResponseDTO>>(FaultMode.REQUESTED_ENTITY_NOT_FOUND, ResponseCodes.INVALID_LOGIN_CREDENTIALS,
                       _messageProvider.GetMessage(ResponseCodes.INVALID_LOGIN_CREDENTIALS));
            }
            if (user.IsLocked(_settings.MaxLoginCount))
            {
                return ErrorResponse.Create<PayloadResponse<AdminLoginResponseDTO>>(
                      FaultMode.CLIENT_INVALID_ARGUMENT,
                      ResponseCodes.ACCOUNT_LOCKED,
                      $"{_messageProvider.GetMessage(ResponseCodes.ACCOUNT_LOCKED)}");

            }
            if (user.ProfileStatus == ProfileStatus.Inactive)
            {
                return ErrorResponse.Create<PayloadResponse<AdminLoginResponseDTO>>(
                      FaultMode.CLIENT_INVALID_ARGUMENT,
                      ResponseCodes.ACCOUNT_DEACTIVATED,
                      $"{_messageProvider.GetMessage(ResponseCodes.ACCOUNT_DEACTIVATED)}");
            }

            var isPasswordEqual = await _cryptoProvider.AreEqualAsync(request.Password, user.Password, user.PasswordSalt);
            if (!isPasswordEqual)
            {
                user.FailedLoginCount++;
                await _adminUserDAO.Update(user);
                return ErrorResponse.Create<PayloadResponse<AdminLoginResponseDTO>>(FaultMode.REQUESTED_ENTITY_NOT_FOUND, ResponseCodes.INVALID_LOGIN_CREDENTIALS,
                       _messageProvider.GetMessage(ResponseCodes.INVALID_LOGIN_CREDENTIALS));
            }

            user.FailedLoginCount = 0;
            user.LastLogin = DateTime.Now;
            var jwtToken = await _cryptoProvider.GenerateJwtToken(new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                LastLogin = user.LastLogin,
                Username = user.Username,
                EmailAddress = user.EmailAddress
            });
            await _adminUserDAO.Update(user);

            response.SetPayload(new AdminLoginResponseDTO
            {
                Access_Token = jwtToken.Access_Token,
                ExpirationTime = jwtToken.ExpirationTime,
                AdminUserDetails = new UserDTO
                {
                    Username = user.Username,
                    EmailAddress = user.EmailAddress,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    LastLogin = user.LastLogin.Value,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender.ToString(),
                    Title = user.Title.ToString()
                }
            });
            response.IsSuccessful = true;
            return response;
        }

        public async Task<PayloadResponse<AdminGetAllUsersResponseDTO>> GetAllUsersAsync()
        {
            var response = new PayloadResponse<AdminGetAllUsersResponseDTO>(false);
            var users = await _userDAO.GetAllUsers();
            if (!users.Any())
            {
                return ErrorResponse.Create<PayloadResponse<AdminGetAllUsersResponseDTO>>(FaultMode.REQUESTED_ENTITY_NOT_FOUND, ResponseCodes.NO_USER_FOUND,
                       _messageProvider.GetMessage(ResponseCodes.NO_USER_FOUND));
            }
            response.SetPayload(new AdminGetAllUsersResponseDTO
            {
                UsersDetails = users.Select(x => new AllUsersDTO
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    LastLogin = x.LastLogin,
                    EmailAddress = x.EmailAddress,
                    Username = x.Username,
                    Address = x.Address,
                    DateOfBirth = x.DateOfBirth,
                    Gender = x.Gender.ToString(),
                    Title = x.Title.ToString(),
                    FailedLoginCount = x.FailedLoginCount,
                    ProfileStatus = x.ProfileStatus,
                    PhoneNumber = x.PhoneNumber
                }).ToList()
            });
            response.IsSuccessful = true;
            return response;
        }

        public async Task<PayloadResponse<AdminGetAllUsersContributionsResponseDTO>> GetAllUsersContributionsAsync()
        {
            var response = new PayloadResponse<AdminGetAllUsersContributionsResponseDTO>(false);
            var userContributions = await _userContributiontDAO.FindAll();
            if (!userContributions.Any())
            {
                return ErrorResponse.Create<PayloadResponse<AdminGetAllUsersContributionsResponseDTO>>(FaultMode.REQUESTED_ENTITY_NOT_FOUND, ResponseCodes.NO_CONTRIBUTION,
                       _messageProvider.GetMessage(ResponseCodes.NO_CONTRIBUTION));
            }
            response.SetPayload(new AdminGetAllUsersContributionsResponseDTO
            {
                UsersContributions = userContributions.Select(x => new AdminUserContributionDTO
                {
                    Amount = x.Amount,
                    ContributionType = x.ContributionType.GetEnumDescription(),
                    DateCreated = x.DateCreated,
                    UserId = x.UserId
                }).ToList()
            });
            response.IsSuccessful = true;
            return response;
        }

        public async Task<BasicResponse> OnboardAdminUserAsync(AdminOnboardingRequestDTO request, string superAccess)
        {
            var response = new BasicResponse(false);
            if (Util.AdminAccess() != superAccess)
            {

                return ErrorResponse.Create<BasicResponse>(FaultMode.REQUESTED_ENTITY_NOT_FOUND, ResponseCodes.INVALID_LOGIN_CREDENTIALS,
                       _messageProvider.GetMessage(ResponseCodes.INVALID_LOGIN_CREDENTIALS));
            }

            var isValidUsername = await _validators.IsUsernameValidAsync(request.Username);
            if (!isValidUsername)
            {
                return ErrorResponse.Create<BasicResponse>(FaultMode.INVALID_OBJECT_STATE, ResponseCodes.INVALID_USERNAME_FORMAT,
                       _messageProvider.GetMessage(ResponseCodes.INVALID_USERNAME_FORMAT));
            }
            var usernameIsAvailable = await _adminUserDAO.UsernameIsAvailable(request.Username);
            if (!usernameIsAvailable)
            {
                return ErrorResponse.Create<BasicResponse>(FaultMode.INVALID_OBJECT_STATE,
                        ResponseCodes.USERNAME_ALREADY_TAKEN, _messageProvider.GetMessage(ResponseCodes.USERNAME_ALREADY_TAKEN));
            };
            var passwordSalt = await _cryptoProvider.GenerateSaltAsync();
            var passwordHash = await _cryptoProvider.GenerateHashAsync(request.NewPassword.Password, passwordSalt);

            try
            {
                var user = new AdminUser
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PasswordSalt = passwordSalt,
                    Password = passwordHash,
                    EmailAddress = request.EmailAddress,
                    PhoneNumber = request.PhoneNumber,
                    Username = request.Username,
                    Gender = request.Gender,
                    Title = request.Title,
                    FailedLoginCount = 0,
                    ProfileStatus = ProfileStatus.Active
                };
                user.Id = await _adminUserDAO.Add(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating admin user {request.Username}");
                return response;
            }
            response.IsSuccessful = true;
            return response;
        }
    }
}
