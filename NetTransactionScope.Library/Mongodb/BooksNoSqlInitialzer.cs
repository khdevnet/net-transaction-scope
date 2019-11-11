using MongoDB.Bson.Serialization;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.Mongodb
{
    public static class BooksNoSqlInitialzer
    {
        public static void Init()
        {
            BsonClassMap.RegisterClassMap<BookBase>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id);
                cm.SetIsRootClass(true);
            });

            BsonClassMap.RegisterClassMap<BookTemp>(cm =>
            {
                cm.AutoMap();
            });

            BsonClassMap.RegisterClassMap<Book>(cm =>
            {
                cm.AutoMap();
            });
        }
    }
}
