using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DAOs.Implementations
{
    public class BaseConnection : IBaseConnection
    {
        protected readonly IDbConnection _dbConnection;
        protected UnitOfWork UnitOfWork;

        public BaseConnection(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public IUnitOfWork Begin()
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            UnitOfWork = new UnitOfWork(_dbConnection);

            return UnitOfWork;
        }

        public void Join(IUnitOfWork token)
        {
            this.UnitOfWork = (UnitOfWork)token;
        }
    }
}
