using System.Transactions;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.Mongodb
{
    public class AddBookOperation : TxOperation
    {
        private readonly BooksNoSqlDbContext _db;
        private readonly Book _book;
        private readonly BookTemp _bookTemp;

        public AddBookOperation(BooksNoSqlDbContext db, Book book)
        {
            _db = db;
            _book = book;
            _bookTemp = new BookTemp
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Path = book.Path
            };
        }

        protected override void PrepareInternal(PreparingEnlistment preparingEnlistment)
        {
            _db.InsertTemp(_bookTemp);
        }

        public override void Commit(Enlistment enlistment)
        {
            _db.TempToBook(_bookTemp.Id);
            enlistment.Done();
        }

        public override void Rollback(Enlistment enlistment)
        {
            DeleteTemp();
            enlistment.Done();
        }

        private void DeleteTemp()
        {
            _db.DeleteTemp(_book.Id);
        }
    }
}
