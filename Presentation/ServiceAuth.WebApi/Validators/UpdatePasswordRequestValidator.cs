using FluentValidation;
using ServiceAuth.WebApi.Models.Requests;

namespace ServiceAuth.WebApi.Validators
{
    public class UpdatePasswordRequestValidator : AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordRequestValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("AccountId обязателен.");

            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .WithMessage("OldPassword обязателен.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("NewPassword обязателен.")
                .MinimumLength(6)
                .WithMessage("Новый пароль должен быть не менее 6 символов.");
        }
    }
}
