using System;
using System.IO;

namespace NetTransactionScope.Tests
{
    public abstract class TestsBase
    {
        protected static string CreateTestFile(string name)
        {
            var applicationBasePath = GetRootPath();
            var testFilePath = Path.Combine(applicationBasePath, name);
            File.AppendAllText(testFilePath, "hello");
            return testFilePath;
        }

        protected static string GetRootPath()
        {
            return AppContext.BaseDirectory;
        }
    }
}
