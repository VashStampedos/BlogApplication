using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Validators.ModelsValidators
{
    public class SubscribeValidator : AbstractValidator<SubscribeModel>
    {
        public SubscribeValidator()
        {
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.SubscriberId).NotNull();
        }
    }
}
