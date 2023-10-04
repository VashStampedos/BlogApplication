using BlogWebAPI.DTO.Auth;
using FluentValidation;

namespace BlogWebAPI.Validators.RequestsValidators.AuthValidators
{
    public class RegisterUserValidator:AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(2);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(4);

        }
    }
}
