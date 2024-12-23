namespace ServiceAuth.Domain.Interfaces
{
    public interface INotificationService
    {
        Task SendEmailAsync(string email, CancellationToken cancellationToken);
    }
}
