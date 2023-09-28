using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Validators
{
    public class ArticleValidator: AbstractValidator<ArticleModel>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title);
        }
    }
}
