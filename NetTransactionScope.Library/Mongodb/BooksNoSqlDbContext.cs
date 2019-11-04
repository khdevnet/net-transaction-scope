using System;
using MongoDB.Driver;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.Mongodb
{
    public class BooksNoSqlDbContext : IDisposable
    {
        private MongoClient client;
        private IClientSessionHandle session;
        private readonly IMongoDatabase db;

        public BooksNoSqlDbContext(string connectionString, string databaseName)
        {
            var mongoSettings = MongoClientSettings.FromConnectionString(connectionString);

            client = new MongoClient(mongoSettings);
            session = client.StartSession();
            db = client.GetDatabase(databaseName);
        }

        public void InsertOne<TEntity>(TEntity entity)
        {
            db.GetCollection<TEntity>(GetCollectionName(entity)).InsertOne(session, entity);
        }

        public IMongoCollection<Book> Books => db.GetCollection<Book>(GetCollectionName<Book>());
        public IMongoCollection<Book> BooksTemp => db.GetCollection<Book>(GetCollectionName<Book>()+"_temp");

        public void DropCollection<T>()
        {
            db.DropCollection(GetCollectionName<T>());
        }

        public void Dispose()
        {
            session.Dispose();
        }

        private static string GetCollectionName<TEntity>(TEntity entity)
        {
            return entity.GetType().Name.ToLower();
        }

        private static string GetCollectionName<TEntity>()
        {
            return typeof(TEntity).Name.ToLower();
        }
    }
}
