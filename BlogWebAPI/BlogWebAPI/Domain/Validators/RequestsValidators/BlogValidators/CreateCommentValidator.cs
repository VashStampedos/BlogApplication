using BlogWebAPI.DTO.Blog;
using FluentValidation;

namespace BlogWebAPI.Domain.Validators.RequestsValidators.BlogValidators
{
    public class CreateCommentValidator:AbstractValidator<CreateCommentRequest>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.ArticleId).NotEmpty();
            RuleFor(x=> x.Description).NotEmpty().WithMessage("Description cant be empty");
        }
    }
}
