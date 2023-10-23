using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Domain.Validators.ModelsValidators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
