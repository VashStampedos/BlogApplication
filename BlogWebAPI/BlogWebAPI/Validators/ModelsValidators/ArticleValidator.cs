using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Validators.ModelsValidators
{
    public class ArticleValidator : AbstractValidator<ArticleModel>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).MinimumLength(1);
            RuleFor(x => x.IdBlog).NotNull();
        }
    }
}
