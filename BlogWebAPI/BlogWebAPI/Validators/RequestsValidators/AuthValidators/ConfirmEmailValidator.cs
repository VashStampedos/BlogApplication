using BlogWebAPI.DTO.Auth;
using FluentValidation;

namespace BlogWebAPI.Validators.RequestsValidators.AuthValidators
{
    public class ConfirmEmailValidator: AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
