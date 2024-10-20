using JWTAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Extensions
{
    public static class AppConfigExtension
    {


        public static WebApplication ConfigureCors(this WebApplication app,IConfiguration configuration)
        {

            app.UseCors();

            return app;

        }


        public static IServiceCollection AddAppConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config.GetSection("AppSettings"));
            return services;
        }



    }
}
