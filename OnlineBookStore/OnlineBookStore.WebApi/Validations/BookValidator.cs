using FluentValidation;
using OnlineBookStore.WebApi.Models;

namespace OnlineBookStore.WebApi.Validations
{
    public class BookValidator : AbstractValidator<BookEditModel>
    {
        public BookValidator()
        {
            RuleFor(a => a.Title)
                .NotEmpty()
                .WithMessage("Tiêu đề bài viết không được để trống")
                .MaximumLength(500)
                .WithMessage("Tiêu đề bài viết tối đa 500 ký tự");

            RuleFor(a => a.Description)
                .NotEmpty()
                .WithMessage("Mô tả không được để trống");
        }
    }
}
