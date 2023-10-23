using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Domain.Validators.ModelsValidators
{
    public class BlogValidator : AbstractValidator<BlogModel>
    {
        public BlogValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.IdUser).NotNull();
            RuleFor(x => x.IdCategory).NotNull();

        }
    }
}
