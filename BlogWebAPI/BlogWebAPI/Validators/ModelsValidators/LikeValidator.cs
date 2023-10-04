using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Validators.ModelsValidators
{
    public class LikeValidator : AbstractValidator<LikeModel>
    {
        public LikeValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.IdUser).NotNull();
            RuleFor(x => x.IdArticle).NotNull();
        }
    }
}
