namespace OnlineBookStore.WebApi.Models
{
    public class BookFilterModel
    {
        public string Keyword { get; set; }
        public string SortColumn { get; set; } = "Id";
        public string SortOrder { get; set; } = "DESC";
    }
}
