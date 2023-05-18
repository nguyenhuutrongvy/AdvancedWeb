namespace OnlineBookStore.WebApi.Models
{
    public class AuthorFilterModel
    {
        public string Name { get; set; }
        public string SortColumn { get; set; } = "Id";
        public string SortOrder { get; set; } = "DESC";
    }
}
