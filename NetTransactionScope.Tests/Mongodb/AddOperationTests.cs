using System;
using System.Diagnostics;
using System.Threading;
using MongoDB.Driver;
using NetTransactionScope.Library.Mongodb;
using Xunit;

namespace NetTransactionScope.Tests.Mongodb
{
    public class AddOperationTests : IDisposable
    {
        public AddOperationTests()
        {
            RunCommand("docker rm mongo-tests");
            RunCommand("docker run --name mongo-tests -p 33381:27017 -d mongo:4");
            Thread.Sleep(1000);
        }

        [Fact]
        public void Add()
        {
            var db = new BooksNoSqlDbContext("mongodb://localhost:33381", "tests-db");
            var bookId = Guid.NewGuid();
            db.InsertOne(new Book
            {
                Id = bookId,
                Title = "test",
                Author = "test",
                Path = "test"
            });

            var book = db.Books.Find(x => x.Id == bookId).FirstOrDefault();

            Assert.Equal(bookId, book.Id);
        }


        private static void RunCommand(string command)
        {
            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            /* execute "dir" */

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            var txt = cmd.StandardOutput.ReadToEnd();
        }

        public void Dispose()
        {
            RunCommand("docker stop mongo-tests");
            RunCommand("docker rm mongo-tests");
        }
    }
}
