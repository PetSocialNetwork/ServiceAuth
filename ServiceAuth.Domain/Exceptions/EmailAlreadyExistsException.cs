namespace ServiceAuth.Domain.Exceptions
{
    /// <summary>
    ///  Исключение типа, если электронный адресс уже существует
    /// </summary>
    public class EmailAlreadyExistsException : DomainException
    {
        public EmailAlreadyExistsException(string? message) : base(message)
        {
        }

        public EmailAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
