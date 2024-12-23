namespace ServiceAuth.Domain.Exceptions
{
    public class PasswordNotChangedException : DomainException
    {
        public PasswordNotChangedException(string? message) : base(message)
        {
        }

        public PasswordNotChangedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
