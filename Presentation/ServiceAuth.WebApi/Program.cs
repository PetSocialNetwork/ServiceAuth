using ServiceAuth.WebApi.Extensions;

namespace ServiceAuth.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ConfigureSettings(builder.Configuration);
            builder.Services.ConfigureServices(builder.Configuration);
            var app = builder.Build();
            app.ConfigureMiddleware();
            app.Run();
        }
    }
}
