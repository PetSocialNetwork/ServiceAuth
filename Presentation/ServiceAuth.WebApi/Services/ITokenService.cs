using ServiceAuth.Domain.Entities;

namespace ServiceAuth.WebApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(Account account);
    }
}
