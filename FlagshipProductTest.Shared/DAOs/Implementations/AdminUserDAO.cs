using Dapper;
using FlagshipProductTest.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DAOs.Implementations
{
    public class AdminUserDAO : BaseConnection, IAdminUserDAO
    {
        private readonly ILogger<AdminUserDAO> _logger;
        public AdminUserDAO(IDbConnection connection, ILogger<AdminUserDAO> logger) : base(connection)
        {
            _logger = logger;
        }
        public async Task<long> Add(AdminUser user)
        {
            long adminCreated = 0;
            try
            {
                var sql = @"INSERT INTO AdminUsers
                            (
                                FirstName,
                                LastName,
                                Username,
                                EmailAddress,
                                Gender,
                                Title,
                                PhoneNumber,
                                DateCreated,
                                Password,
                                PasswordSalt,
                                FailedLoginCount,
                                ProfileStatus
                             )
                             VALUES
                             (
                                @FirstName,
                                @LastName,
                                @Username,
                                @EmailAddress,
                                @Gender,
                                @Title,
                                @PhoneNumber,
                                @DateCreated,
                                @Password,
                                @PasswordSalt,
                                @FailedLoginCount,
                                @ProfileStatus
                            );
                             SELECT SCOPE_IDENTITY()";

                adminCreated = await _dbConnection.QueryFirstOrDefaultAsync<long>(sql, user, UnitOfWork?.GetDbTransaction());
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Failed to Add Admin User {user.Username}");
            }
            return adminCreated;
        }

        public async Task<AdminUser> FindByUsername(string username)
        {
            var result = await _dbConnection.
                         QueryFirstOrDefaultAsync<AdminUser>
                         ("SELECT * FROM AdminUsers c WHERE c.Username = @Username",
                           new AdminUser { Username = username }
                         );

            return result;
        }

        public async Task Update(AdminUser user)
        {
            string sql = @"UPDATE AdminUsers
                          SET   LastLogin = @LastLogin,
                                FailedLoginCount = @FailedLoginCount
                          WHERE Id = @Id";
            var token = UnitOfWork?.GetDbTransaction();
            await _dbConnection.ExecuteAsync(sql, user, token);
        }

        public async Task<bool> UsernameIsAvailable(string username)
        {
            if (_dbConnection.State == ConnectionState.Closed) _dbConnection.Open();
            return await _dbConnection.QueryFirstOrDefaultAsync<string>("SELECT Username FROM AdminUsers c WHERE c.Username = @Username",
                   new AdminUser { Username = username }) == null;
        }
    }
}
