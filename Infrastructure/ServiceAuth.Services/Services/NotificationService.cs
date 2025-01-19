using PetSocialNetwork.ServiceNotification;
using ServiceAuth.Domain.Interfaces;

namespace ServiceAuth.NswagClients.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationApiClient _notificationApiClient;

        public NotificationService(INotificationApiClient notificationApiClient)
        {
            _notificationApiClient = notificationApiClient;
        }

        public async Task SendEmailAsync(string email, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            var request = new EmailRequest()
            {
                //TODO: отправлять в красивом формате
                Message = "Поздравляем с успешной регистрацией",
                RecepientEmail = email,
                Subject = "Регистрация"
            };
            await _notificationApiClient.SendEmailAsync(request, cancellationToken);
        }
    }
}
