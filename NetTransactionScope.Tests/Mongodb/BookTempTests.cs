using System;
using MongoDB.Driver;
using NetTransactionScope.Library.Entity;
using NetTransactionScope.Library.Mongodb;
using Xunit;

namespace NetTransactionScope.Tests.Mongodb
{
    public class BookTempTests : IClassFixture<MongoDbClassFixture>
    {

        [Fact]
        public void InsertTempBookSuccessfullTest()
        {
            var bookId = Guid.NewGuid();
            var db = new BooksNoSqlDbContext();

            var book = new BookTemp
            {
                Id = bookId,
                Title = "test",
                Author = "test",
                Path = "test"
            };

            db.InsertTemp(book);

            var bookTemp = db.GetTemp(bookId);
            var newBook = db.Books.Find(x => x.Id == bookId).FirstOrDefault();
            Assert.Equal(bookId, bookTemp.Id);
            Assert.Null(newBook);
        }

        [Fact]
        public void UpdateBookTempToBookSuccessfullTest()
        {
            var bookId = Guid.NewGuid();
            var db = new BooksNoSqlDbContext();

            var book = new BookTemp
            {
                Id = bookId,
                Title = "test",
                Author = "test",
                Path = "test"
            };

            db.InsertTemp(book);

           

            db.TempToBook(bookId);

            var addedBook = db.Books.Find(x=>x.Id == bookId).FirstOrDefault();
            var bookTemp = db.GetTemp(bookId);

            Assert.NotNull(addedBook);
            Assert.Equal(bookId, addedBook.Id);
            Assert.Null(bookTemp);
        }
    }
}
