namespace ServiceAuth.Domain.Exceptions
{
    /// <summary>
    /// Исключение типа, если аккаунт не найден
    /// </summary>
    public class AccountNotFoundException : DomainException
    {
        public AccountNotFoundException(string? message) : base(message)
        {
        }

        public AccountNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
