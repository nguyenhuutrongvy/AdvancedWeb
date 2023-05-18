using FluentValidation;
using OnlineBookStore.WebApi.Models;

namespace OnlineBookStore.WebApi.Validations
{
    public class AuthorValidator : AbstractValidator<AuthorEditModel>
    {
        public AuthorValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Tên tác giả không được để trống")
                .MaximumLength(255)
                .WithMessage("Tên tác giả tối đa 255 ký tự");
        }
    }
}
