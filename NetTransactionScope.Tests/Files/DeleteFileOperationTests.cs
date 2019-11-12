using System;
using System.IO;
using System.Transactions;
using NetTransactionScope.Library.Files;
using Xunit;

namespace NetTransactionScope.Tests.Files
{
    public class DeleteFileOperationTests: TestsBase
    {
        [Fact]
        public void CompleteTransactionFileDeletedTest()
        {
            string testFilePath = CreateTestFile("CompleteTransaction.txt");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //Create an enlistment object
                    new DeleteFileOperation(testFilePath);
                    //Perform transactional work here.

                    scope.Complete();
                }
            }
            catch (TransactionException ex)
            {
                Console.WriteLine(ex);
            }
            catch
            {
                Console.WriteLine("Cannot complete transaction");
            }

            Assert.False(File.Exists(testFilePath));
        }

        [Fact]
        public void NotCompleteTransactionFileRollbackTest()
        {
            string testFilePath = CreateTestFile("NotCompleteTransaction.txt");
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    //Create an enlistment object
                    new DeleteFileOperation(testFilePath);

                    //Perform transactional work here.
                }
            }
            catch (TransactionException ex)
            {
                Console.WriteLine(ex);
            }
            catch
            {
                Console.WriteLine("Cannot complete transaction");
            }

            Assert.True(File.Exists(testFilePath));
            File.Delete(testFilePath);
        }

        private static string CreateTestFile(string name)
        {
            var applicationBasePath = GetRootPath();
            var testFilePath = Path.Combine(applicationBasePath, name);
            File.AppendAllText(testFilePath, "hello");
            return testFilePath;
        }
    }
}
