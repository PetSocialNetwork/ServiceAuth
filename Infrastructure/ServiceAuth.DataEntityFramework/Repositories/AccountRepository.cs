using Microsoft.EntityFrameworkCore;
using ServiceAuth.Domain.Entities;
using ServiceAuth.Domain.Interfaces;

namespace ServiceAuth.DataEntityFramework.Repositories
{
    public class AccountRepository : EFRepository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext appDbContext) : base(appDbContext) { }
        public async Task<Account?> FindAccountByEmail(string email, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(email);
            return await Entities.SingleOrDefaultAsync(it => it.Email.Value == email, cancellationToken);
        }

        public async Task<Account?> FindAccountById(Guid id, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(it => it.Id == id, cancellationToken);
        }



    }
}
