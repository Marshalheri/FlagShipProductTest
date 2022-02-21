using FlagshipProductTest.Shared.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DTOs.Admin
{
    public class AdminGetAllUsersContributionsResponseDTO
    {
        public List<AdminUserContributionDTO> UsersContributions { get; set; }
    }

    public class AdminUserContributionDTO : UserContributionDTO
    {
        public long UserId { get; set; }
    }
}
