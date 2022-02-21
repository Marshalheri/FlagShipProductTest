using System.ComponentModel;

namespace FlagshipProductTest.Shared.Models
{
    public class UserContribution : BaseModel
    {
        public long UserId { get; set; }
        public decimal Amount { get; set; }
        public ContributionType ContributionType { get; set; }
    }

    public enum ContributionType
    {
        [Description("Savings")]
        Savings = 1,
        [Description("End-Well")]
        EndWell
    }
}
