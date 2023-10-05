using BlogWebAPI.DTO.Blog;
using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Validators.RequestsValidators.BlogValidators
{
    public class CreateBlogValidator:AbstractValidator<CreateBlogRequest>
    {
        public CreateBlogValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
