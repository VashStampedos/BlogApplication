using BlogWebAPI.DTO.Blog;
using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Domain.Validators.RequestsValidators.BlogValidators
{
    public class CreateBlogValidator:AbstractValidator<CreateBlogRequest>
    {
        public CreateBlogValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cant be empty");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Blog needs category");
        }
    }
}
