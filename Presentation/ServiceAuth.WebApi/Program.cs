using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ServiceAuth.DataEntityFramework;
using ServiceAuth.DataEntityFramework.Repositories;
using ServiceAuth.Domain.Interfaces;
using ServiceAuth.Domain.Services;
using ServiceAuth.IdentityPasswordHasherLib;
using ServiceAuth.NswagClients.Services;
using ServiceAuth.WebApi.Configurations;
using ServiceAuth.WebApi.Filters;
using ServiceAuth.WebApi.Services;

namespace ServiceAuth.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            JwtConfig? jwtConfig = builder.Configuration
               .GetRequiredSection("JwtConfig")
               .Get<JwtConfig>();
            if (jwtConfig is null)
            {
                throw new InvalidOperationException("JwtConfig is not configured");
            }
            builder.Services.AddSingleton(jwtConfig);

            builder.Services.AddCors();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<CentralizedExceptionHandlingFilter>();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "v1" });     
                //options.UseAllOfToExtendReferenceSchemas();
                //string pathToXmlDocs = Path.Combine(AppContext.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".xml");
                //options.IncludeXmlComments(pathToXmlDocs, true);
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
               options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped(typeof(IRepositoryEF<>), typeof(EFRepository<>));
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<IApplicationPasswordHasher, IdentityPasswordHasher>();
            
            var app = builder.Build();

            app.UseCors(policy =>
            {
                policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            });
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
