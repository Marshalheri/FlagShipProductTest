using System;
using System.Collections.Generic;

namespace FlagshipProductTest.Shared.DTOs.User
{
    public class GetUserContributionResponseDTO
    {
        public List<UserContributionPerTypeDTO> UserContributionDetails { get; set; }
        public decimal TotalContributionAmount { get; set; }
    }

    public class UserContributionDTO
    {
        public decimal Amount { get; set; }
        public string ContributionType { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class UserContributionPerTypeDTO
    {
        public decimal TotalTypeContributionAmount { get; set; }
        public string ContributionType { get; set; }
        public List<UserContributionDTO> UserContributions { get; set; }
    }
}
