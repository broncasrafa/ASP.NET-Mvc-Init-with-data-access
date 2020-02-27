using System.Collections.Generic;
using System.Data;
using System.Threading;
using DataAccess.Providers.ADO.Interfaces;

namespace DataAccess.Providers.ADO
{
    public class DbContext
    {
        private readonly IDbConnection _connection;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IDbProvider _dbProvider;
        private readonly ReaderWriterLockSlim _rwLockSlim = new ReaderWriterLockSlim();
        private readonly LinkedList<UnitOfWork> _unitOfWork = new LinkedList<UnitOfWork>();

        public DbContext(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _dbProvider = _connectionFactory.GetProvider();
            _connection = _connectionFactory.CreateConnection();
        }

        public IDbProvider DbProvider
        {
            get { return _dbProvider; }
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var transaction = _connection.BeginTransaction();
            var uow = new UnitOfWork(transaction, RemoveTransaction, RemoveTransaction);

            _rwLockSlim.EnterWriteLock();
            _unitOfWork.AddLast(uow);
            _rwLockSlim.ExitWriteLock();
            return uow;
        }

        private void RemoveTransaction(UnitOfWork uow)
        {
            _rwLockSlim.EnterWriteLock();
            _unitOfWork.Remove(uow);
            _rwLockSlim.ExitWriteLock();
        }

        public IDbCommand CreateCommand()
        {
            var command = _connection.CreateCommand();
            command.CommandTimeout = 0;

            _rwLockSlim.EnterReadLock();

            if (_unitOfWork.Count > 0)
                command.Transaction = _unitOfWork.First.Value.Transaction;

            _rwLockSlim.ExitReadLock();

            return command;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
