using JWTAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Extensions
{
    public  static class EntityFrameworkExtension
    {

        public static IServiceCollection InjectDBContext(this IServiceCollection services,IConfiguration config) 
        {
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(config.GetConnectionString("devdb")));
            return services;
        }


    }
}
