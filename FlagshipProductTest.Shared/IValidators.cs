using System;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared
{
    public interface IValidators
    {
        Task<bool> IsUsernameValidAsync(string userName); 
        Task<bool> IsValidDobAsync(DateTime DOB);
    }
}
