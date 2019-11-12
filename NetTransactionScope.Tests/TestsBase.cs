using System;

namespace NetTransactionScope.Tests
{
    public abstract class TestsBase
    {
        protected static string GetRootPath()
        {
            return AppContext.BaseDirectory;
        }
    }
}
