using OnlineBookStore.Core.Contracts;

namespace OnlineBookStore.Core.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Book> Books { get; set; }
    }
}
