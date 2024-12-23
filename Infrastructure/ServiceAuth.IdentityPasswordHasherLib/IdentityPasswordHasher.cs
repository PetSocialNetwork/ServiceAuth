using Microsoft.AspNetCore.Identity;
using ServiceAuth.Domain.Interfaces;

namespace ServiceAuth.IdentityPasswordHasherLib
{
    public class IdentityPasswordHasher : IApplicationPasswordHasher
    {
        private readonly PasswordHasher<object> _passwordHasher = new();
        private readonly object _fake = new();
        public string HashPassword(string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(password));
            return _passwordHasher.HashPassword(_fake, password);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword, out bool rehashNeeded)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(hashedPassword));
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(providedPassword));

            var result = _passwordHasher.VerifyHashedPassword(_fake, hashedPassword, providedPassword);
            rehashNeeded = result == PasswordVerificationResult.SuccessRehashNeeded;
            return result != PasswordVerificationResult.Failed;

        }
    }
}
