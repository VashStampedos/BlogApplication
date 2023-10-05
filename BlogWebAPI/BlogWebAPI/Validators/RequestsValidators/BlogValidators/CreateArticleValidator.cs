using BlogWebAPI.DTO.Blog;
using FluentValidation;

namespace BlogWebAPI.Validators.RequestsValidators.BlogValidators
{
    public class CreateArticleValidator: AbstractValidator<CreateArticleRequest>
    {
        public CreateArticleValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Photo).NotEmpty();
            RuleFor(x => x.BlogId).NotEmpty();
        }
    }
}
