using FlagshipProductTest.Client.Filters;
using FlagshipProductTest.Core;
using FlagshipProductTest.Shared.DTOs;
using FlagshipProductTest.Shared.DTOs.User;
using FlagshipProductTest.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlagshipProductTest.Client.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : RootController
    {
        private readonly IUserService _userOnboardingService;
        public UserController(IUserService userOnboardingService)
        {
            _userOnboardingService = userOnboardingService;
        }

        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("auth")]
        [ProducesResponseType(typeof(UserLoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [TypeFilter(typeof(ValidateRequestBodyFilter<UserLoginRequestDTO>))]
        public async Task<IActionResult> AuthenticatUser([FromBody] UserLoginRequestDTO request)
        {
            var result = await _userOnboardingService.AuthenticatUserAsync(request);

            if (!result.IsSuccessful)
            {
                return CreateResponse(result.Error, result.FaultType);
            }
            return Ok(result.GetPayload());
        }

        /// <summary>
        /// Onboard a new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("onboarding")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [TypeFilter(typeof(ValidateRequestBodyFilter<UserOnboardingRequestDTO>))]
        public async Task<IActionResult> OnboardUser([FromBody] UserOnboardingRequestDTO request)
        {
            var result = await _userOnboardingService.OnboardUserAsync(request);

            if (!result.IsSuccessful)
            {
                return CreateResponse(result.Error, result.FaultType);
            }
            return Ok();
        }

        /// <summary>
        /// Add user contribution
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("contribution/add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [TypeFilter(typeof(ValidateRequestBodyFilter<AddContributionRequestDTO>))]
        public async Task<IActionResult> AddUserContribution([FromBody] AddContributionRequestDTO request)
        {
            request.Username = User.Identity.GetUsername();
            var result = await _userOnboardingService.AddUserContributionAsync(request);

            if (!result.IsSuccessful)
            {
                return CreateResponse(result.Error, result.FaultType);
            }
            return Ok();
        }

        /// <summary>
        /// Get user contributions
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("contribution/get")]
        [ProducesResponseType(typeof(GetUserContributionResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserContributionDetails()
        {
            var result = await _userOnboardingService.GetUserContributionDetailsAsync(User.Identity.GetUsername());

            if (!result.IsSuccessful)
            {
                return CreateResponse(result.Error, result.FaultType);
            }
            return Ok(result.GetPayload());
        }
    }
}
