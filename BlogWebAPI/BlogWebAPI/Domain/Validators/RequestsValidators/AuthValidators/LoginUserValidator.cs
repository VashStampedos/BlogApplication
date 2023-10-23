using BlogWebAPI.DTO.Auth;
using FluentValidation;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BlogWebAPI.Domain.Validators.RequestsValidators.AuthValidators
{
    public class LoginUserValidator: AbstractValidator<LogInRequest>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email address should include '@' symbol");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(4).WithMessage("Username length should be more then 2 symbols");
        }
    }
}
