using FlagshipProductTest.Shared.Models;
using System;
using System.Text.Json.Serialization;

namespace FlagshipProductTest.Shared.DTOs.User
{
    public class AddContributionRequestDTO : BaseRequestValidatorDTO
    {
        public decimal Amount { get; set; }
        public ContributionType ContributionType { get; set; }
        [JsonIgnore]
        public string Username { get; set; }
        public override bool IsValid(out string problemSource)
        {
            problemSource = string.Empty;
            if (Amount <= 0)
            {
                problemSource = "Contribution Amount";
                return false;
            }
            if (!Enum.IsDefined(typeof(ContributionType), ContributionType))
            {
                problemSource = "Contribution Type";
                return false;
            }
            return true;
        }
    }
}
