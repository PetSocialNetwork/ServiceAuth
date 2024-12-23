namespace ServiceAuth.Domain.Exceptions
{
    public class InvalidEmailException : DomainException
    {
        public InvalidEmailException(string? message) : base(message)
        {
        }

        public InvalidEmailException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
