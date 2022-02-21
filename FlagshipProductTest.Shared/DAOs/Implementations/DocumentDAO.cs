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
    public class DocumentDAO : BaseConnection, IDocumentDAO
    {
        private readonly ILogger<DocumentDAO> _logger;
        public DocumentDAO(ILogger<DocumentDAO> logger, IDbConnection connection) : base(connection)
        {
            _logger = logger;
        }

        public async Task<long> Add(Document document)
        {
            long documentId = 0;
            try
            {
                var sql = @"INSERT INTO Documents
                            (
                                UserId,
                                DocumentType,
                                RawData,
                                Extension,
                                DateCreated
                             )
                             VALUES
                             (
                                @UserId,
                                @DocumentType,
                                @RawData,
                                @Extension,
                                @DateCreated,
                            );
                             SELECT SCOPE_IDENTITY()";

                documentId = await _dbConnection.QueryFirstOrDefaultAsync<long>(sql, document, UnitOfWork?.GetDbTransaction());
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Failed to Add User document of type {document.DocumentType} userid: {document.UserId}");
            }
            return documentId;
        }
    }
}
