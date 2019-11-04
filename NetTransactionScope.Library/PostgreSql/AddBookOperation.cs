using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.PostgreSql
{
    public class AddBookOperation : TxOperation
    {
        private readonly IBooksSqlDbContext _db;
        private IDbContextTransaction _transaction;
        private readonly Book _book;

        public AddBookOperation(IBooksSqlDbContext db, Book book)
        {
            _db = db;
            _book = book;
        }

        public override void PrepareInternal(PreparingEnlistment preparingEnlistment)
        {
            _transaction = _db.Database.BeginTransaction();
            _db.Books.Add(_book);
            _db.SaveChanges();
        }

        public override void Commit(Enlistment enlistment)
        {
            _transaction.Commit();
            _transaction.Dispose();
            enlistment.Done();
        }

        public override void Rollback(Enlistment enlistment)
        {
            _transaction.Rollback();
            _transaction.Dispose();
            enlistment.Done();
        }
    }
}
