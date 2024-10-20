namespace JWTAuthentication.Extensions
{
    public  static class SwaggerExtension
    {

        public static IServiceCollection AddSwaggerUI(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
           services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }


        public static WebApplication ConfigureSwaggerExplorer(this WebApplication app) 
        {

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.UseDeveloperExceptionPage<>(options=> options.);

            }

            return app;

        }
    }
}
