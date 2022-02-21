using FlagshipProductTest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DAOs
{
    public interface IAdminUserDAO : IBaseConnection
    {
        Task<long> Add(AdminUser user);
        Task<AdminUser> FindByUsername(string username);
        Task Update(AdminUser user);
        Task<bool> UsernameIsAvailable(string username);
    }
}
