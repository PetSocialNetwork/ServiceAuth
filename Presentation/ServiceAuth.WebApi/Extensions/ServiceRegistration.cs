using PetSocialNetwork.ServiceNotification;
using PetSocialNetwork.ServiceUser;

namespace ServiceAuth.WebApi.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            services.AddServiceClient<INotificationApiClient, NotificationApiClient>("NotificationService");
            services.AddServiceClient<IUserProfileApiClient, UserProfileApiClient>("UserService");

            return services;
        }
    }
}
