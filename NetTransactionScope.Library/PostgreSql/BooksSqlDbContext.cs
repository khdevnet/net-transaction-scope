using Microsoft.EntityFrameworkCore;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.PostgreSql
{
    public class BooksSqlDbContext : DbContext
    {
        public const string ConnectionStringTemplate = @"Host={0};Port={1};Database={2};Username={3};Password={4};";// Enlist=true
        public const string Password = "123456";
        public const string User = "postgres";
        public const string Port = "33382";
        public const string Host = "127.0.0.1";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(string.Format(ConnectionStringTemplate, Host, Port, "book_library", User, Password));
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildBook(modelBuilder);
        }

        private static void BuildBook(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("book");

            modelBuilder.Entity<Book>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Book>()
                .Property(p => p.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(p => p.Author)
                .HasColumnName("author")
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(p => p.Path)
                .HasColumnName("path")
                .IsRequired();
        }
    }
}
