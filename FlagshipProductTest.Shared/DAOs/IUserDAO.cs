using FlagshipProductTest.Shared.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DAOs
{
    public interface IUserDAO : IBaseConnection
    {
        Task<long> Add(User user);
        Task<User> FindByUsername(string username);
        Task Update(User user);
        Task<bool> UsernameIsAvailable(string username);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
