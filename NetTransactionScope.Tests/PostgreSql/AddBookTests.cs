using System;
using NetTransactionScope.Library.Entity;
using NetTransactionScope.Library.PostgreSql;
using Xunit;

namespace NetTransactionScope.Tests.PostgreSql
{
    public class AddBookTests : IClassFixture<PostgreSqlDbClassFixture>
    {
        [Fact]
        public void AddBookAddedTest()
        {
            var bookId = Guid.NewGuid();
            using (var db = new BooksSqlDbContext())
            {
                var book = new Book
                {
                    Id = bookId,
                    Title = "test",
                    Author = "test",
                    Path = "test"
                };

                db.Books.Add(book);
                db.SaveChanges();
                var addedBook = db.Books.Find(bookId);
                Assert.Equal(bookId, addedBook.Id);
            }

            using (var db = new BooksSqlDbContext())
            {
                var addedBook = db.Books.Find(bookId);
                db.Books.Remove(addedBook);
                db.SaveChanges();
            }
        }
    }
}
