using System;
using System.IO;
using System.Text;
using System.Transactions;
using NetTransactionScope.Library.Files;
using NSubstitute;
using NSubstitute.Extensions;
using Xunit;

namespace NetTransactionScope.Tests.Files
{
    public class CreateFileOperationTests : TestsBase
    {
        private readonly string destFilePath = Path.Combine(GetRootPath(), "dest.txt");
        private readonly byte[] sourceFile = Encoding.ASCII.GetBytes("hshshshshshshshshs");

        [Fact]
        public void CompleteTransactionFileCreatedTest()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                new CreateFileOperation(destFilePath, sourceFile);

                scope.Complete();
            }

            Assert.True(File.Exists(destFilePath));
            File.Delete(destFilePath);
        }

        [Fact]
        public void NotCompleteTransactionFileRollbackTest()
        {
            var fileStorage = Substitute.ForPartsOf<FileStorage>();
            fileStorage.Configure()
                .When(x => x.CreateFile(Arg.Any<string>(), Arg.Any<byte[]>()))
                .Do(x => throw new IOException());

            Assert.Throws<TransactionAbortedException>(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    new CreateFileOperation(destFilePath, sourceFile, fileStorage);

                    scope.Complete();
                }
            });

            Assert.False(File.Exists(destFilePath));
        }
    }
}
