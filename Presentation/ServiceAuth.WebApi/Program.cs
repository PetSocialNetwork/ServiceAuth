using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceAuth.DataEntityFramework;
using ServiceAuth.DataEntityFramework.Repositories;
using ServiceAuth.Domain.Interfaces;
using ServiceAuth.Domain.Services;
using ServiceAuth.IdentityPasswordHasherLib;
using ServiceAuth.NswagClients.Services;
using ServiceAuth.WebApi.Configurations;
using ServiceAuth.WebApi.Extensions;
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
            builder.Services.AddControllers();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<CentralizedExceptionHandlingFilter>();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddClientServices();
            builder.Services.AddHttpClient();

            //builder.Services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1.0", new OpenApiInfo
            //    {
            //        Title = "Service Notification",
            //        Description = $"Версия сборки:",
            //        Version = "v1.0"
            //    });

            //    options.UseAllOfToExtendReferenceSchemas();

            //    string pathToXmlDocs = Path.Combine(AppContext.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".xml");
            //    options.IncludeXmlComments(pathToXmlDocs, true);
            //});

            builder.Services.AddDbContext<AppDbContext>(options =>
               options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped(typeof(IRepositoryEF<>), typeof(EFRepository<>));
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<IApplicationPasswordHasher, IdentityPasswordHasher>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     IssuerSigningKey = new SymmetricSecurityKey(jwtConfig.SigningKeyBytes),
                     ValidateIssuerSigningKey = true,
                     ValidateLifetime = true,
                     RequireExpirationTime = true,
                     RequireSignedTokens = true,
                     ValidateAudience = true,
                     ValidateIssuer = true,
                     ValidAudiences = new[] { jwtConfig.Audience },
                     ValidIssuer = jwtConfig.Issuer
                 };
             });

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
