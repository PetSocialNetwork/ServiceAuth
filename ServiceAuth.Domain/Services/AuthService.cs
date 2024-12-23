using Service_Auth.Entities;
using ServiceAuth.Domain.Entities;
using ServiceAuth.Domain.Exceptions;
using ServiceAuth.Domain.Interfaces;

namespace ServiceAuth.Domain.Services
{
    public class AuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IApplicationPasswordHasher _passwordHasher;
        private readonly IUserProfileService _userProfileService;
        private readonly INotificationService _notificationService;

        public AuthService
            (IAccountRepository accountRepository,
            IUserProfileService userProfileService,
            INotificationService notificationApiClient,
            IApplicationPasswordHasher passwordHasher)
        {
            _accountRepository = accountRepository
                ?? throw new ArgumentNullException(nameof(accountRepository));
            _passwordHasher = passwordHasher
                 ?? throw new ArgumentNullException(nameof(passwordHasher));
            _userProfileService = userProfileService
                ?? throw new ArgumentNullException(nameof(userProfileService));
            _notificationService = notificationApiClient
                ?? throw new ArgumentNullException(nameof(notificationApiClient));
            ;
        }

        public async Task<Account> Register
            (string email, string password, CancellationToken cancellationToken)
        {
            //Транзакция
            ArgumentException.ThrowIfNullOrEmpty(nameof(email));
            ArgumentException.ThrowIfNullOrEmpty(nameof(password));

            var existedAccount = await _accountRepository.FindAccountByEmail(email, cancellationToken);
            if (existedAccount is not null)
            {
                throw new EmailAlreadyExistsException("Aккаунт с таким email уже существует.");
            }
            var account = new Account(Guid.NewGuid(), new Email(email), EncryptPassword(password));
            await _accountRepository.Add(account, cancellationToken);

            var userResponse = await _userProfileService.AddUserProfileAsync(account.Id, cancellationToken);
            //TODO: отправляем запрос на добавление профиля животного и передаем туда userProfileId
            await _notificationService.SendEmailAsync(email, cancellationToken);

            //await transaction.CommitAsync(cancellationToken);
            //TODO: добавить в claims дополнительные данные
            return account;
        }

        public async Task<Account> LoginByPassword(string email, string password, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(email));
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(password));

            var account = await _accountRepository.FindAccountByEmail(email, cancellationToken);
            if (account is null)
            {
                throw new AccountNotFoundException("Аккаунт с таким e-mail не найден.");
            }

            var isPasswordValid =
                _passwordHasher.VerifyHashedPassword
                (account.HashedPassword, password, out bool rehash);

            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Неверный пароль.");
            }

            if (rehash)
            {
                await RehashPassword(password, account, cancellationToken);
            }

            return account;
        }

        public async Task DeleteAccountAsync(Guid id, CancellationToken cancellationToken)
        {
            var existedAccount = await _accountRepository.FindAccountById(id, cancellationToken);
            if (existedAccount is null)
            {
                throw new AccountNotFoundException("Аккаунт с таким e-mail не найден.");
            }
            await _accountRepository.Delete(existedAccount, cancellationToken);
        }

        public async Task UpdatePasswordAsync(Guid id, string oldPassword, string newPassword, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(oldPassword);
            ArgumentException.ThrowIfNullOrWhiteSpace(newPassword);

            if (_passwordHasher.HashPassword(oldPassword) == _passwordHasher.HashPassword(newPassword))
            {
                throw new PasswordNotChangedException("Новый пароль не должен совпадать со старым.");
            }

            var existedAccount = await _accountRepository.FindAccountById(id, cancellationToken);

            if (existedAccount is null)
            {
                throw new AccountNotFoundException("Аккаунт с таким e-mail не найден.");
            }

            var isPasswordValid =
                _passwordHasher.VerifyHashedPassword
                (existedAccount.HashedPassword, oldPassword, out bool rehash);

            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Неверный пароль.");
            }

            existedAccount.HashedPassword = _passwordHasher.HashPassword(newPassword);
            await _accountRepository.Update(existedAccount, cancellationToken);
        }

        public async Task ResetPasswordAsync(Guid id, string newPassword, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newPassword);

            var existedAccount = await _accountRepository.FindAccountById(id, cancellationToken);
            if (existedAccount is null)
            {
                throw new AccountNotFoundException("Аккаунт с таким e-mail не найден.");
            }
            existedAccount.HashedPassword = _passwordHasher.HashPassword(newPassword);
            await _accountRepository.Update(existedAccount, cancellationToken);
        }

        private async Task RehashPassword(string password, Account account, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(account));
            ArgumentException.ThrowIfNullOrEmpty(nameof(password));
            account.HashedPassword = EncryptPassword(password);
            await _accountRepository.Update(account, cancellationToken);
        }

        private string EncryptPassword(string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(password));
            return _passwordHasher.HashPassword(password);
        }
    }
}
