using MongoDB.Driver;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.Mongodb
{
    public class BooksNoSqlDbContext
    {
        private MongoClient client;
        private readonly IMongoDatabase db;

        public BooksNoSqlDbContext()
        {
            var connectionString = "mongodb://localhost:33381";
            var databaseName = "tests-db";

            client = new MongoClient(MongoClientSettings.FromConnectionString(connectionString));
            db = client.GetDatabase(databaseName);
        }

        public virtual IMongoCollection<Book> Books => db.GetCollection<Book>(GetCollectionName<Book>());

        public virtual IMongoCollection<Book> BooksTemp => db.GetCollection<Book>(GetCollectionName<Book>() + "_temp");

        private static string GetCollectionName<TEntity>() => typeof(TEntity).Name.ToLower();
    }
}
