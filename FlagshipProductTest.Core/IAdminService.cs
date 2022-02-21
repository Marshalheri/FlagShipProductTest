using FlagshipProductTest.Shared.DTOs;
using FlagshipProductTest.Shared.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Core
{
    public interface IAdminService
    {
        Task<BasicResponse> OnboardAdminUserAsync(AdminOnboardingRequestDTO request, string superAccess);
        Task<PayloadResponse<AdminLoginResponseDTO>> AuthenticatAdminUserAsync(AdminLoginRequestDTO request);
        Task<PayloadResponse<AdminGetAllUsersContributionsResponseDTO>> GetAllUsersContributionsAsync();
        Task<PayloadResponse<AdminGetAllUsersResponseDTO>> GetAllUsersAsync();
    }
}
