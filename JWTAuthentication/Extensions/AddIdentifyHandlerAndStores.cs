using JWTAuthentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;

namespace JWTAuthentication.Extensions
{
    public static class IdentityExtensions
    {

        public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection Services)
        {
            Services.AddIdentityApiEndpoints<AppUser>()
                     .AddEntityFrameworkStores<AppDbContext>();
            return Services;

        }


        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection Services)
        {
            Services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;

                });
            return Services;
        }

        public static IServiceCollection AddIdentityAuth(this IServiceCollection services,IConfiguration config)
        {

               services.AddAuthentication(x => x.DefaultAuthenticateScheme =
                x.DefaultChallengeScheme =
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(y =>
                {
                y.SaveToken = false;
                y.TokenValidationParameters = new TokenValidationParameters
                {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["AppSettings:JWTSecret"]!))

                };
                });

            return services;
        }



        public static WebApplication AddIdentityAuthMiddleware(this WebApplication app)
        {


            app.UseAuthentication();

            app.UseAuthorization();


            return app;

        }





    }
}

public class RegistrationModel
{
    public string Email { get; set; }
    public string fullName { get; set; }

    public string Password { get; set; }

}


public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }

}
