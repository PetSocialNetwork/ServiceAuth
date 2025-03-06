using ServiceAuth.Domain.Entities;

namespace ServiceAuth.Domain.Interfaces
{
    public interface IAccountRepository : IRepositoryEF<Account>
    {
        Task<Account?> FindAccountByEmail(string email, CancellationToken cancellationToken);
        Task<Account?> FindAccountById(Guid id, CancellationToken cancellationToken);
        Task<bool> IsRegisterUserAsync(string email, CancellationToken cancellationToken);
    }
}
