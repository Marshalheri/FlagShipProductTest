using Dapper;
using FlagshipProductTest.Shared.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DAOs.Implementations
{
    public class UserContributionDAO : BaseConnection, IUserContributionDAO
    {
        private readonly ILogger<UserContributionDAO> _logger;
        public UserContributionDAO(IDbConnection connection, ILogger<UserContributionDAO> logger) : base(connection)
        {
            _logger = logger;
        }
        public async Task<long> Add(UserContribution userContribution)
        {
            long contributionId = 0;
            try
            {
                var sql = @"INSERT INTO UserContributions
                            (
                                UserId,
                                ContributionType,
                                Amount,
                                DateCreated
                             )
                             VALUES
                             (
                                @UserId,
                                @ContributionType,
                                @Amount,
                                @DateCreated
                            );
                             SELECT SCOPE_IDENTITY()";

                contributionId = await _dbConnection.QueryFirstOrDefaultAsync<long>(sql, userContribution, UnitOfWork?.GetDbTransaction());
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Failed to Add User contribution of type {userContribution.ContributionType} userid: {userContribution.UserId}");
            }
            return contributionId;
        }

        public async Task<IEnumerable<UserContribution>> FindAll()
        {
            return await _dbConnection.
                             QueryAsync<UserContribution>
                             ("SELECT * FROM UserContributions");
        }

        public async Task<IEnumerable<UserContribution>> FindByUserId(long userId)
        {
            return await _dbConnection.
                          QueryAsync<UserContribution>
                          ("SELECT * FROM UserContributions c WHERE c.UserId = @UserId",
                            new UserContribution { UserId = userId }
                          );
        }
    }
}
