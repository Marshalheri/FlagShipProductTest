using Dapper;
using FlagshipProductTest.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DAOs.Implementations
{
    public class UserDAO : BaseConnection, IUserDAO
    {
        private readonly ILogger<UserDAO> _logger;
        public UserDAO(IDbConnection connection, ILogger<UserDAO> logger) : base(connection)
        {
            _logger = logger;
        }

        public async Task<long> Add(User user)
        {
            long customerCreated = 0;
            try
            {
                var sql = @"INSERT INTO Users
                            (
                                FirstName,
                                LastName,
                                Username,
                                EmailAddress,
                                Address,
                                DateOfBirth,
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
                                @Address,
                                @DateOfBirth,
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

                customerCreated = await _dbConnection.QueryFirstOrDefaultAsync<long>(sql, user, UnitOfWork?.GetDbTransaction());
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Failed to Add User {user.Username}");
            }
            return customerCreated;
        }
        public async Task<User> FindByUsername(string username)
        {
            var result = await _dbConnection.
                        QueryFirstOrDefaultAsync<User>
                        ("SELECT * FROM Users c WHERE c.Username = @Username",
                          new User { Username = username }
                        );

            return result;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _dbConnection.QueryAsync<User>("SELECT * FROM Users");
        }

        public async Task Update(User user)
        {
            string sql = @"UPDATE Users
                          SET   LastLogin = @LastLogin,
                                FailedLoginCount = @FailedLoginCount
                          WHERE Id = @Id";
            var token = UnitOfWork?.GetDbTransaction();
            await _dbConnection.ExecuteAsync(sql, user, token);
        }

        public async Task<bool> UsernameIsAvailable(string username)
        {
            if (_dbConnection.State == ConnectionState.Closed) _dbConnection.Open();
            return await _dbConnection.QueryFirstOrDefaultAsync<string>("SELECT Username FROM Users c WHERE c.Username = @Username",
                   new User { Username = username }) == null;
        }


    }
}
