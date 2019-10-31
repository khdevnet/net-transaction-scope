using System;
using MongoDB.Bson.Serialization.Attributes;

namespace NetTransactionScope.Library.Mongodb
{
    public class Book
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public string Author { get; set; }
    }
}
