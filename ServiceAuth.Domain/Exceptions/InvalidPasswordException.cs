namespace ServiceAuth.Domain.Exceptions
{
    /// <summary>
    ///  Исключение типа, если пароль указан неверно
    /// </summary>
    public class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException(string? message) : base(message)
        {
        }

        public InvalidPasswordException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
