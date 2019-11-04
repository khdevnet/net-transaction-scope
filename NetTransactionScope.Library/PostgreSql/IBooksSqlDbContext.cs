using Microsoft.EntityFrameworkCore;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.PostgreSql
{
    public interface IBooksSqlDbContext
    {
        DbSet<Book> Books { get; set; }

        Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }

        int SaveChanges();
    }
}