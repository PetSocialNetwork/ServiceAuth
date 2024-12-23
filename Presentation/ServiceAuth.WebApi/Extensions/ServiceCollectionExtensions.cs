namespace ServiceAuth.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceClient<TClientInterface, TClientImplementation>(
            this IServiceCollection services,
            string serviceConfigKey)
            where TClientImplementation : class, TClientInterface
            where TClientInterface : class
        {
            services.AddScoped(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var baseAddress = configuration.GetValue<string>($"Services:{serviceConfigKey}")
                                  ?? throw new InvalidOperationException($"Configuration value for Services:{serviceConfigKey} not found.");


                var constructor = typeof(TClientImplementation).GetConstructor(new[] { typeof(string), typeof(HttpClient) });

                if (constructor == null)
                {
                    throw new InvalidOperationException($"Type {typeof(TClientImplementation).FullName} must have a constructor accepting a string and HttpClient.");
                }


                return (TClientInterface)constructor.Invoke(new object[] { baseAddress, httpClient });

            });

            return services;
        }
    }
}
