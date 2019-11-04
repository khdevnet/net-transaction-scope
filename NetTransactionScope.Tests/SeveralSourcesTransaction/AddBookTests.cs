using System;
using System.IO;
using System.Text;
using System.Transactions;
using MongoDB.Driver;
using NetTransactionScope.Library.Entity;
using NetTransactionScope.Library.Files;
using NetTransactionScope.Library.Mongodb;
using NetTransactionScope.Library.PostgreSql;
using NSubstitute;
using NSubstitute.Extensions;
using Xunit;
using AddBookMongoDbOperation = NetTransactionScope.Library.Mongodb.AddBookOperation;
using AddBookPostgreSqlDbOperation = NetTransactionScope.Library.PostgreSql.AddBookOperation;

namespace NetTransactionScope.Tests.SeveralSourcesTransaction
{
    public class AddBookTests : TestsBase, IClassFixture<MongoDbClassFixture>, IClassFixture<PostgreSqlDbClassFixture>
    {
        private readonly string destFilePath = Path.Combine(GetRootPath(), "dest.txt");
        private readonly byte[] sourceFile = Encoding.ASCII.GetBytes("hshshshshshshshshs");

        [Fact]
        public void AddBookInSingleTransactionSuccessfullTest()
        {
            var bookId = Guid.NewGuid();
            Book book = CreateBookEntity(bookId);

            using (TransactionScope scope = new TransactionScope())
            {
                CreateBookSql(book);
                CreateBookNoSql(book);
                CreateBookFile();

                scope.Complete();
            }

            AssertNoSqlBookAdded(bookId);
            AssertSqlBookAdded(bookId);
            AssertBookFileAdded();

            CleanupBookFile(destFilePath);
            CleanupBookMongoDb(bookId);
            CleanupBookPostgreSqlDb(bookId);
        }

        [Fact]
        public void AddBookInSingleTransactionCreateFileFailedTest()
        {
            var bookId = Guid.NewGuid();
            Book book = CreateBookEntity(bookId);

            var fileStorage = Substitute.ForPartsOf<FileStorage>();
            fileStorage.Configure()
                .When(x => x.CreateFile(Arg.Any<string>(), Arg.Any<byte[]>()))
                .Do(x => throw new IOException());

            using (TransactionScope scope = new TransactionScope())
            {
                CreateBookSql(book);
                CreateBookNoSql(book);
                new CreateFileOperation(destFilePath, sourceFile, fileStorage);
                scope.Complete();
            }

            //Assert.Throws<IOException>(() =>
            //{


            //});

            AssertNoSqlBookNotAdded(bookId);
            AssertSqlBookNotAdded(bookId);
            AssertBookFileNotAdded();
        }

        private void CreateBookFile()
        {
            new CreateFileOperation(destFilePath, sourceFile);
        }

        private static void CreateBookNoSql(Book book)
        {
            new AddBookMongoDbOperation(new BooksNoSqlDbContext(), book);
        }


        private static void CreateBookSql(Book book)
        {
            new AddBookPostgreSqlDbOperation(new BooksSqlDbContext(), book);
        }

        private static Book CreateBookEntity(Guid bookId)
        {
            var book = new Book
            {
                Id = bookId,
                Title = "test",
                Author = "test",
                Path = "test"
            };
            return book;
        }

        private static void CleanupBookFile(string destFilePath)
        {
            File.Delete(destFilePath);
        }

        private static void CleanupBookMongoDb(Guid bookId)
        {
            var db = new BooksNoSqlDbContext();
            db.Books.DeleteOne(x => x.Id == bookId);
        }

        private void CleanupBookPostgreSqlDb(Guid bookId)
        {
            using (var db = new BooksSqlDbContext())
            {
                var addedBook = db.Books.Find(bookId);
                db.Books.Remove(addedBook);
                db.SaveChanges();
            }
        }

        private void AssertBookFileAdded()
        {
            Assert.True(File.Exists(destFilePath));
        }

        private void AssertBookFileNotAdded()
        {
            Assert.False(File.Exists(destFilePath));
        }

        private static void AssertNoSqlBookAdded(Guid bookId)
        {
            var addedBookNoSql = new BooksNoSqlDbContext().Books.Find(x => x.Id == bookId).FirstOrDefault();

            Assert.Equal(bookId, addedBookNoSql.Id);
        }

        private static void AssertNoSqlBookNotAdded(Guid bookId)
        {
            var addedBookNoSql = new BooksNoSqlDbContext().Books.Find(x => x.Id == bookId).FirstOrDefault();

            Assert.Null(addedBookNoSql);
        }

        private static void AssertSqlBookAdded(Guid bookId)
        {
            using (var db = new BooksSqlDbContext())
            {
                var addedBookSql = db.Books.Find(bookId);
                Assert.Equal(bookId, addedBookSql.Id);
            }
        }

        private static void AssertSqlBookNotAdded(Guid bookId)
        {
            using (var db = new BooksSqlDbContext())
            {
                var addedBookSql = db.Books.Find(bookId);
                Assert.Null(addedBookSql);
            }
        }
    }
}
