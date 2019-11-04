using System.Transactions;
using MongoDB.Driver;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.Mongodb
{
    public class AddBookOperation : IEnlistmentNotification
    {
        private readonly BooksNoSqlDbContext _db;
        private readonly Book _book;

        public AddBookOperation(BooksNoSqlDbContext db, Book book)
        {
            _db = db;
            _book = book;
            Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {
                _db.Books.InsertOne(_book);

                //If work finished correctly, reply prepared
                preparingEnlistment.Prepared();
            }
            catch
            {
                // otherwise, do a ForceRollback
                preparingEnlistment.ForceRollback();
            }
        }

        public void Commit(Enlistment enlistment)
        {
            //Declare done on the enlistment
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public void Rollback(Enlistment enlistment)
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
