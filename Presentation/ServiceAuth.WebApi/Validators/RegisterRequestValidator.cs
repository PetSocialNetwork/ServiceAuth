using FluentValidation;
using ServiceAuth.WebApi.Models.Requests;

namespace ServiceAuth.WebApi.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email обязателен.")
                .EmailAddress()
                .WithMessage("Некорректный формат email.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Пароль обязателен.")
                .MinimumLength(6)
                .WithMessage("Пароль должен быть не менее 6 символов.");
        }
    }
}
