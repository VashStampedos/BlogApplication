using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Domain.Validators.ModelsValidators
{
    public class CategoryValidator : AbstractValidator<CategoryModel>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotEmpty();

        }
    }
}
