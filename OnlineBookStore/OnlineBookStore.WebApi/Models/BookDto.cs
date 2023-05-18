namespace OnlineBookStore.WebApi.Models
{
    public class BookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public string File { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}
