using BlogWebAPI.DTO.Auth;
using FluentValidation;

namespace BlogWebAPI.Domain.Validators.RequestsValidators.AuthValidators
{
    public class RegisterUserValidator:AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(2).WithMessage("Username length should be more then 2 symbols");
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(4).WithMessage("Password length should be more then 2 symbols");

        }
    }
}
