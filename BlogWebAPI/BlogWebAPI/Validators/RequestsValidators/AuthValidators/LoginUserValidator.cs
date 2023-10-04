using BlogWebAPI.DTO.Auth;
using FluentValidation;

namespace BlogWebAPI.Validators.RequestsValidators.AuthValidators
{
    public class LoginUserValidator: AbstractValidator<LogInRequest>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(4);
        }
    }
}
