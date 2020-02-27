using System;
using System.Data;
using DataAccess.Providers.ADO.Interfaces;

namespace DataAccess.Providers.ADO
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbTransaction _transaction;
        private readonly Action<UnitOfWork> _rolledback;
        private readonly Action<UnitOfWork> _committed;

        public IDbTransaction Transaction { get; private set; }

        public UnitOfWork(IDbTransaction transaction, Action<UnitOfWork> rolledback, Action<UnitOfWork> committed)
        {
            Transaction = transaction;
            _transaction = transaction;
            _rolledback = rolledback;
            _committed = committed;
        }

        public void Dispose()
        {
            if (_transaction == null) return;

            _transaction.Rollback();
            _transaction.Dispose();
            _rolledback(this);
            _transaction = null;
        }

        public void SaveChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Não pode chamar save changes duas vezes.");

            _transaction.Commit();
            _committed(this);
            _transaction = null;
        }
    }
}
