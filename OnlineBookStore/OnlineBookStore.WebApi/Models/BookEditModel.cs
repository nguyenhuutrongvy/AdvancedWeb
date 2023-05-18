using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnlineBookStore.WebApi.Models
{
    public class BookEditModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public string File { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}

/*using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.WebApi.Models
{
    public class BookEditModel
    {
        [DisplayName("Tiêu đề")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(500, ErrorMessage = "Tiêu đề tối đa 500 ký tự")]
        public string Title { get; set; }

        [DisplayName("Nội dung")]
        [Required]
        public string Description { get; set; }

        [DisplayName("Chọn hình ảnh")]
        public IFormFile Cover { get; set; }
        
        [DisplayName("Chọn tập tin")]
        public IFormFile File { get; set; }

        [DisplayName("Tác giả")]
        [Required]
        public int AuthorId { get; set; }

        [DisplayName("Chủ đề")]
        [Required]
        public int CategoryId { get; set; }

        public static async ValueTask<BookEditModel> BindAsync(HttpContext context)
        {
            var form = await context.Request.ReadFormAsync();
            return new BookEditModel()
            {
                Title = form["Title"],
                Description = form["Description"],
                Cover = form.Files["Cover"],
                File = form.Files["File"],
                CategoryId = int.Parse(form["CategoryId"]),
                AuthorId = int.Parse(form["AuthorId"])
            };
        }
    }
}*/
