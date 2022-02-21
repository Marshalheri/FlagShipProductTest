using FlagshipProductTest.Shared.DTOs;
using FlagshipProductTest.Shared.DTOs.User;
using System.Threading.Tasks;

namespace FlagshipProductTest.Core
{
    public interface IUserService
    {
        Task<BasicResponse> OnboardUserAsync(UserOnboardingRequestDTO request);
        Task<PayloadResponse<UserLoginResponseDTO>> AuthenticatUserAsync(UserLoginRequestDTO request);
        Task<BasicResponse> AddUserContributionAsync(AddContributionRequestDTO request);
        Task<PayloadResponse<GetUserContributionResponseDTO>> GetUserContributionDetailsAsync(string username);
    }
}
