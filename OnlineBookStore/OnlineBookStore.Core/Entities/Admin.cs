using OnlineBookStore.Core.Contracts;

namespace OnlineBookStore.Core.Entities
{
    public class Admin : IEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
