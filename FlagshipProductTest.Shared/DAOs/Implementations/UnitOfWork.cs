using System.Data;

namespace FlagshipProductTest.Shared.DAOs.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly IDbTransaction _dbTransaction;
        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbTransaction = dbConnection.BeginTransaction();
        }
        public void Commit()
        {
            _dbTransaction.Commit();
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
        }

        public IDbTransaction GetDbTransaction()
        {
            return _dbTransaction;
        }
    }
}
