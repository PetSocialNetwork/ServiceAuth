using FluentValidation;
using ServiceAuth.WebApi.Configurations;

namespace ServiceAuth.WebApi.Validators
{
    public class JwtConfigValidator : AbstractValidator<JwtConfig>
    {
        public JwtConfigValidator()
        {
            RuleFor(x => x.SigningKey)
                .NotEmpty()
                .WithMessage("SigningKey не может быть пустым.")
                .Must(key => !string.IsNullOrWhiteSpace(key))
                .WithMessage("SigningKey не может быть только пробелами.");

            RuleFor(x => x.LifeTime)
                .GreaterThan(TimeSpan.Zero)
                .WithMessage("Время жизни токена должно быть больше нуля.");

            RuleFor(x => x.Audience)
                .NotEmpty()
                .WithMessage("Audience обязательно.")
                .MaximumLength(100)
                .WithMessage("Audience не должно превышать 100 символов.");

            RuleFor(x => x.Issuer)
                .NotEmpty()
                .WithMessage("Issuer обязательно.")
                .MaximumLength(100)
                .WithMessage("Issuer не должно превышать 100 символов.");
        }
    }
}
