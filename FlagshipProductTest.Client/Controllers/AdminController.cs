using FlagshipProductTest.Client.Filters;
using FlagshipProductTest.Core;
using FlagshipProductTest.Shared.DTOs;
using FlagshipProductTest.Shared.DTOs.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlagshipProductTest.Client.Controllers
{
    [ApiController]
    [Route("api/v1/admin")]
    public class AdminController : RootController
    {
        private readonly IAdminService _service;
        public AdminController(IAdminService service)
        {
            _service = service;
        }

        /// <summary>
        /// Authenticate an admin user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("auth")]
        [ProducesResponseType(typeof(AdminLoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [TypeFilter(typeof(ValidateRequestBodyFilter<AdminLoginRequestDTO>))]
        public async Task<IActionResult> AuthenticatAdminUser([FromBody] AdminLoginRequestDTO request)
        {
            var result = await _service.AuthenticatAdminUserAsync(request);
            if (!result.IsSuccessful)
            {
                return CreateResponse(result.Error, result.FaultType);
            }
            return Ok(result.GetPayload());
        }

        /// <summary>
        /// Onboard a new admin user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("onboarding")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [TypeFilter(typeof(ValidateRequestBodyFilter<AdminOnboardingRequestDTO>))]
        public async Task<IActionResult> OnboardUser([FromBody] AdminOnboardingRequestDTO request, [FromHeader] string Super_Access)
        {
            var result = await _service.OnboardAdminUserAsync(request, Super_Access);
            if (!result.IsSuccessful)
            {
                return CreateResponse(result.Error, result.FaultType);
            }
            return Ok();
        }
        
        /// <summary>
        /// Get all customers contributions
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("customer/contributions/all")]
        [ProducesResponseType(typeof(AdminGetAllUsersContributionsResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsersContributions()
        {
            var result = await _service.GetAllUsersContributionsAsync();
            if (!result.IsSuccessful)
            {
                return CreateResponse(result.Error, result.FaultType);
            }
            return Ok(result.GetPayload());
        }
        
        /// <summary>
        /// Get all customers details
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("customer/details/all")]
        [ProducesResponseType(typeof(AdminGetAllUsersResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _service.GetAllUsersAsync();
            if (!result.IsSuccessful)
            {
                return CreateResponse(result.Error, result.FaultType);
            }
            return Ok(result.GetPayload());
        }
    }
}
