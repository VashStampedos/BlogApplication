using BlogWebAPI.DTO.Blog;
using FluentValidation;

namespace BlogWebAPI.Validators.RequestsValidators.BlogValidators
{
    public class CreateCommentValidator:AbstractValidator<CreateCommentRequest>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.ArticleId).NotEmpty();
            RuleFor(x=> x.Description).NotEmpty();
        }
    }
}
