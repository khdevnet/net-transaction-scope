using System;
using MongoDB.Bson;
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

        public virtual IMongoCollection<Book> Books => db.GetCollection<Book>(GetCollectionName<Book>()).OfType<Book>();

        public virtual BookTemp GetTemp(Guid bookId) => BooksTemp.Find(b => b.Id == bookId).FirstOrDefault();

        public virtual void InsertTemp(BookTemp bookTemp) => BooksTemp.InsertOne(bookTemp);

        public virtual void DeleteTemp(Guid bookId)
        {
            if (BooksTemp.CountDocuments(x => x.Id == bookId) > 0)
            {
                BooksTemp.DeleteOne(x => x.Id == bookId);
            }
        }

        public virtual void TempToBook(Guid bookId)
        {
            var discriminators = new BsonArray
            {
                nameof(Book),
                nameof(BookBase)
            };

            var update = Builders<BsonDocument>.Update.Set("_t", discriminators);
            var collection = db.GetCollection<BsonDocument>(GetCollectionName<Book>());
            FilterDefinition<BsonDocument> filters = Builders<BsonDocument>.Filter.Eq("_id", bookId);
            collection.UpdateOne(filters, update);
        }
        private IMongoCollection<BookTemp> BooksTemp => db.GetCollection<BookTemp>(GetCollectionName<Book>()).OfType<BookTemp>();

        private static string GetCollectionName<TEntity>() => typeof(TEntity).Name.ToLower();
    }
}
