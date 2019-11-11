using System;

namespace NetTransactionScope.Library.Entity
{
    public abstract class BookBase
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public string Author { get; set; }
    }
}
