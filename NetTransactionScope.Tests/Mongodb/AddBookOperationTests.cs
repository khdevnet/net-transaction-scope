using System;
using System.Transactions;
using MongoDB.Driver;
using NetTransactionScope.Library.Entity;
using NetTransactionScope.Library.Mongodb;
using Xunit;

namespace NetTransactionScope.Tests.Mongodb
{
    public class AddBookOperationTests : IClassFixture<MongoDbClassFixture>
    {

        [Fact]
        public void AddBookOperationBookAdded()
        {
            var bookId = Guid.NewGuid();
            var db = new BooksNoSqlDbContext();

            using (TransactionScope scope = new TransactionScope())
            {

                var book = new Book
                {
                    Id = bookId,
                    Title = "test",
                    Author = "test",
                    Path = "test"
                };
                new AddBookOperation(db, book);
                scope.Complete();
            }

            var addedBook = db.Books.Find(x => x.Id == bookId).FirstOrDefault();

            Assert.Equal(bookId, addedBook.Id);
        }

        [Fact]
        public void AddBookOperationBookNotAdded()
        {
            var bookId = Guid.NewGuid();
            var db = new BooksNoSqlDbContext();

            using (TransactionScope scope = new TransactionScope())
            {

                var book = new Book
                {
                    Id = bookId,
                    Title = "test",
                    Author = "test",
                    Path = "test"
                };
                new AddBookOperation(db, book);
            }

            var addedBook = db.Books.Find(x => x.Id == bookId).FirstOrDefault();

            Assert.Null(addedBook);
        }
    }
}
