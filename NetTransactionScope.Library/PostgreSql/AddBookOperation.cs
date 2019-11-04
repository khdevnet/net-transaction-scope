using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.PostgreSql
{
    public class AddBookOperation : IEnlistmentNotification
    {
        private readonly IBooksSqlDbContext _db;
        private IDbContextTransaction _transaction;
        private readonly Book _book;

        public AddBookOperation(IBooksSqlDbContext db, Book book)
        {
            _db = db;
            _book = book;
            Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {
                _transaction = _db.Database.BeginTransaction();
                _db.Books.Add(_book);
                _db.SaveChanges();
                preparingEnlistment.Prepared();
            }
            catch (Exception e)
            {
                preparingEnlistment.ForceRollback();
            }
        }

        public void Commit(Enlistment enlistment)
        {
            _transaction.Commit();
            _transaction.Dispose();
            enlistment.Done();
        }

        public void Rollback(Enlistment enlistment)
        {
            _transaction.Rollback();
            _transaction.Dispose();
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
        }
    }
}
