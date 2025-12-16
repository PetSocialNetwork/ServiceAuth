using ServiceAuth.Domain.Entities;
using ServiceAuth.Domain.Exceptions;
using ServiceAuth.Domain.Interfaces;

namespace ServiceAuth.Domain.Services
{
    public class AuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IApplicationPasswordHasher _passwordHasher;

        public AuthService
            (IAccountRepository accountRepository,
            IApplicationPasswordHasher passwordHasher)
        {
            _accountRepository = accountRepository
                ?? throw new ArgumentNullException(nameof(accountRepository));
            _passwordHasher = passwordHasher
                 ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<Account> Register
            (Account account, CancellationToken cancellationToken)
        {          
            var existedAccount = await _accountRepository
                .FindAccountByEmail(account.Email.ToString(), cancellationToken);
            if (existedAccount is not null)
            {
                throw new EmailAlreadyExistsException("Aккаунт с таким email уже существует.");
            }

            account.Password = EncryptPassword(account.Password);
            await _accountRepository.Add(account, cancellationToken);
            return account;
        }

        public async Task<Account> LoginByPassword(Account account, CancellationToken cancellationToken)
        {
            var existedAccount = await GetAccountByEmailOrThrowAsync(account.Email.ToString(), cancellationToken);

            var isPasswordValid = VerifyPassword
                (existedAccount.Password, account.Password, out bool rehash);

            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Неверный пароль.");
            }

            if (rehash)
            {
                await RehashPassword(account.Password, existedAccount, cancellationToken);
            }

            return existedAccount;
        }

        public async Task DeleteAccountAsync
            (Guid id, CancellationToken cancellationToken)
        {
            var existedAccount = await _accountRepository.FindAccountById(id, cancellationToken)
                ?? throw new AccountNotFoundException("Аккаунт с таким e-mail не найден.");
            await _accountRepository.Delete(existedAccount, cancellationToken);
        }

        public async Task ResetPasswordAsync
            (Account account, CancellationToken cancellationToken)
        {
            var existedAccount = await GetAccountByEmailOrThrowAsync(account.Email.ToString(), cancellationToken);

            var isPasswordValid = VerifyPassword
                (existedAccount.Password, account.Password, out bool rehash);

            if (isPasswordValid)
            {
                throw new PasswordNotChangedException("Новый пароль не должен совпадать со старым.");
            }

            existedAccount.Password = EncryptPassword(account.Password);
            await _accountRepository.Update(existedAccount, cancellationToken);
        }

        public async Task UpdatePasswordAsync
            (Guid id, string oldPassword, string newPassword, CancellationToken cancellationToken)
        {
            if (oldPassword == newPassword)
            {
                throw new PasswordNotChangedException("Новый пароль не должен совпадать со старым.");
            }

            var existedAccount = await _accountRepository.FindAccountById(id, cancellationToken)
                ?? throw new AccountNotFoundException("Аккаунт с таким идентификатором не найден.");

            var isPasswordValid = VerifyPassword
                         (existedAccount.Password, oldPassword, out bool rehash);

            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Старый пароль указан неверно.");
            }

            existedAccount.Password = _passwordHasher.HashPassword(newPassword);
            await _accountRepository.Update(existedAccount, cancellationToken);
        }

        public async Task<bool> IsTheSameUserPasswordAsync
            (Account account, CancellationToken cancellationToken)
        {
            return await IsTheSamePassword(account, cancellationToken);
        }

        public async Task<bool> IsRegisterUserAsync
            (string email, CancellationToken cancellationToken)
        {
            return await _accountRepository.IsRegisterUserAsync(email, cancellationToken);
        }
        public async Task<Account> GetAccountByIdAsync
            (Guid id, CancellationToken cancellationToken)
        {
            return await _accountRepository.FindAccountById(id, cancellationToken)
                ?? throw new AccountNotFoundException("Аккаунт с таким e-mail не найден.");
        }

        private async Task<Account> GetAccountByEmailOrThrowAsync
            (string email, CancellationToken cancellationToken)
        {
            return await _accountRepository.FindAccountByEmail(email, cancellationToken)
                ?? throw new AccountNotFoundException("Аккаунт с таким e-mail не найден.");
        }

        private async Task<bool> IsTheSamePassword
            (Account account, CancellationToken cancellationToken)
        {
            var existedAccount = await GetAccountByEmailOrThrowAsync(account.Email.ToString(), cancellationToken);

            var isPasswordValid = VerifyPassword
                         (existedAccount.Password, account.Password, out bool rehash);
            return isPasswordValid;
        }

        private async Task RehashPassword
            (string password, Account account, CancellationToken cancellationToken)
        {
            account.Password = EncryptPassword(password);
            await _accountRepository.Update(account, cancellationToken);
        }

        private string EncryptPassword(string password)
        {
            return _passwordHasher.HashPassword(password);
        }
        private bool VerifyPassword(string hashedPassword, string plainPassword, out bool needsRehash)
        {
            return _passwordHasher.VerifyHashedPassword(hashedPassword, plainPassword, out needsRehash);
        }
    }
}
