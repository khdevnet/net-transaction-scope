using System.Transactions;
using MongoDB.Driver;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.Mongodb
{
    public class AddBookOperation : TxOperation
    {
        private readonly BooksNoSqlDbContext _db;
        private readonly Book _book;

        public AddBookOperation(BooksNoSqlDbContext db, Book book)
        {
            _db = db;
            _book = book;
        }

        public override void PrepareInternal(PreparingEnlistment preparingEnlistment)
        {
            _db.Books.InsertOne(_book);
        }

        public override void Rollback(Enlistment enlistment)
        {
            DeleteAddedItem();
            enlistment.Done();
        }

        private void DeleteAddedItem()
        {
            if (_db.Books.CountDocuments(x => x.Id == _book.Id) > 0)
            {
                _db.Books.DeleteOne(x => x.Id == _book.Id);
            }
        }
    }
}
