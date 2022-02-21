using FlagshipProductTest.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DAOs
{
    public interface IUserContributionDAO : IBaseConnection
    {
        Task<long> Add(UserContribution userContribution);
        Task<IEnumerable<UserContribution>> FindByUserId(long userId);
        Task<IEnumerable<UserContribution>> FindAll();
    }
}
