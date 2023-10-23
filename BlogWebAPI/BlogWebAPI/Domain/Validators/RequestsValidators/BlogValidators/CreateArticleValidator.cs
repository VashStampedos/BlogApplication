using BlogWebAPI.DTO.Blog;
using FluentValidation;

namespace BlogWebAPI.Domain.Validators.RequestsValidators.BlogValidators
{
    public class CreateArticleValidator: AbstractValidator<CreateArticleRequest>
    {
        public CreateArticleValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(3).WithMessage("Title length should be more then 3 symbols");
            RuleFor(x => x.Description).NotEmpty().MinimumLength(10).WithMessage("Description length should be more then 10 symbols");
            RuleFor(x => x.Photo).NotEmpty();
            RuleFor(x => x.BlogId).NotEmpty();
        }
    }
}
