using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.PostgreSql
{
    public interface IBooksSqlDbContext
    {
        DbSet<Book> Books { get; set; }

        DatabaseFacade Database { get; }

        int SaveChanges();
    }
}