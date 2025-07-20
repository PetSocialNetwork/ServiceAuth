using FluentValidation;
using ServiceAuth.WebApi.Models.Requests;

namespace ServiceAuth.WebApi.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email не заполнен.")
                .EmailAddress()
                .WithMessage("Некорректный формат email.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password не заполнен.")
                .MinimumLength(6)
                .WithMessage("Пароль должен быть не менее 6 символов.");
        }
    }
}
