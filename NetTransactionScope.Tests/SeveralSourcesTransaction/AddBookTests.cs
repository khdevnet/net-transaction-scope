using System;
using System.IO;
using System.Text;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NetTransactionScope.Library.Entity;
using NetTransactionScope.Library.Files;
using NetTransactionScope.Library.Mongodb;
using NetTransactionScope.Library.PostgreSql;
using Xunit;

namespace NetTransactionScope.Tests.SeveralSourcesTransaction
{
    public class AddBookTests : TestsBase, IClassFixture<MongoDbClassFixture>
    {
        [Fact]
        public void AddBookWithFileInSingleTransactionSuccessfullTest()
        {
            string destFilePath = Path.Combine(GetRootPath(), "dest.txt");
            byte[] sourceFile = Encoding.ASCII.GetBytes("hshshshshshshshshs");

            var bookId = Guid.NewGuid();
            var db = new BooksNoSqlDbContext("mongodb://localhost:33381", "tests-db");
            var book = new Book
            {
                Id = bookId,
                Title = "test",
                Author = "test",
                Path = "test"
            };

            using (TransactionScope scope = new TransactionScope())
            {
                new AddBookOperation(db, book);
                new CreateFileOperation(destFilePath, sourceFile);

                scope.Complete();
            }

            var addedBook = db.Books.Find(x => x.Id == bookId).FirstOrDefault();

            Assert.Equal(bookId, addedBook.Id);
            Assert.True(File.Exists(destFilePath));
            File.Delete(destFilePath);
            db.Books.DeleteOne(x => x.Id == bookId);
        }

        [Fact]
        public void AddBookWithFileInSingleTransactionMongoDbFailRevertFileTest()
        {
            string destFilePath = Path.Combine(GetRootPath(), "dest.txt");
            byte[] sourceFile = Encoding.ASCII.GetBytes("hshshshshshshshshs");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    new CreateFileOperation(destFilePath, sourceFile);

                    throw new Exception("MongoTransactionFail");
                }
            }
            catch (Exception e)
            {
                Assert.False(File.Exists(destFilePath));
            }
        }

        [Fact]
        public void AddBookWithFileInSingleTransactionCreateFilFailRevertMongoTest()
        {
            var bookId = Guid.NewGuid();
            var db = new BooksNoSqlDbContext("mongodb://localhost:33381", "tests-db");
            var book = new Book
            {
                Id = bookId,
                Title = "test",
                Author = "test",
                Path = "test"
            };

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    new AddBookOperation(db, book);

                    throw new Exception("CreateFileFail");
                }
            }
            catch (Exception e)
            {
                var addedBook = db.Books.Find(x => x.Id == bookId).FirstOrDefault();

                Assert.Null(addedBook);
            }
        }
    }
}
