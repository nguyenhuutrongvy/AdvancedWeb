using FluentValidation;
using OnlineBookStore.WebApi.Models;

namespace OnlineBookStore.WebApi.Validations
{
    public class CategoryValidator : AbstractValidator<CategoryEditModel>
    {
        public CategoryValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Tên chủ đề không được để trống")
                .MaximumLength(255)
                .WithMessage("Tên chủ đề tối đa 255 ký tự");
        }
    }
}
