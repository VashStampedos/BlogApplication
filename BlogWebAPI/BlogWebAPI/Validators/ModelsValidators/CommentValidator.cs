using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Validators.ModelsValidators
{
    public class CommentValidator : AbstractValidator<CommentModel>
    {
        public CommentValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.IdUser).NotNull();
            RuleFor(x => x.IdArticle).NotNull();
        }
    }
}
