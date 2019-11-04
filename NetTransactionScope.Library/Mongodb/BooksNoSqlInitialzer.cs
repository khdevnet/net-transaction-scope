using MongoDB.Bson.Serialization;
using NetTransactionScope.Library.Entity;

namespace NetTransactionScope.Library.Mongodb
{
    public static class BooksNoSqlInitialzer
    {
        public static void Init()
        {
            BsonClassMap.RegisterClassMap<Book>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id);
            });
        }
    }
}
