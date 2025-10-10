using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ServiceAuth.DataEntityFramework.Repositories;
using ServiceAuth.DataEntityFramework;
using ServiceAuth.Domain.Interfaces;
using ServiceAuth.WebApi.Configurations;
using ServiceAuth.Domain.Services;
using ServiceAuth.IdentityPasswordHasherLib;
using ServiceAuth.WebApi.Services;
using FluentValidation;
using ServiceAuth.WebApi.Filters;
using FluentValidation.AspNetCore;

namespace ServiceAuth.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            AddInfrastructure(services, configuration);
            AddApplicationComponents(services, configuration);
        }

        public static void ConfigureSettings
           (this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration
              .GetRequiredSection("JwtConfig")
              .Get<JwtConfig>()
              ?? throw new InvalidOperationException("JwtConfig is not configured");
            services.AddSingleton(jwtConfig);
        }

        public static void ConfigureMiddleware(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }

        private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Scoped);
            services.AddControllers(options =>
            {
                options.Filters.Add<CentralizedExceptionHandlingFilter>();
            });

            services.AddFluentValidationAutoValidation();
            services.AddCors();
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "v1" });
            });
            services.AddHttpClient();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void AddApplicationComponents(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services, configuration);
            AddDomainServices(services);
        }

        private static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Postgres")));
            services.AddScoped(typeof(IRepositoryEF<>), typeof(EFRepository<>));
            services.AddScoped<IAccountRepository, AccountRepository>();
        }

        private static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<AuthService>();
            services.AddScoped<IApplicationPasswordHasher, IdentityPasswordHasher>();
        }

        public static void AddServiceClient<TClientInterface, TClientImplementation>(
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


                var constructor = typeof(TClientImplementation).GetConstructor([typeof(string), typeof(HttpClient)]);

                if (constructor == null)
                {
                    throw new InvalidOperationException($"Type {typeof(TClientImplementation).FullName} must have a constructor accepting a string and HttpClient.");
                }

                return (TClientInterface)constructor.Invoke([baseAddress, httpClient]);

            });
        }
    }
}
