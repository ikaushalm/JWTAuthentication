using Azure;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding Identity  Endpoints
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
 


}
    );

builder.Services.AddDbContext<AppDbContext>(option=>option.UseSqlServer(builder.Configuration.GetConnectionString("devdb")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage<>(options=> options.);

}

app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.MapGroup("/api").
    MapIdentityApi<AppUser>();


app.MapPost("/api/SignUp", [AllowAnonymous] async (UserManager<AppUser> usrmgr, [FromBody] RegistrationModel regmodel) =>
{
AppUser user = new AppUser{
    FullName=regmodel.fullName,
    Email=regmodel.Email,
    UserName=regmodel.Email

};
    var response=await usrmgr.CreateAsync(user,regmodel.Password);

    if (response.Succeeded)
    {
        return Results.Ok(response);
    }
    else
    {
        return Results.BadRequest(response);    
    }

});



app.Run();

public class RegistrationModel
{
    public string Email { get; set; }
    public string fullName { get; set; }

    public string Password { get; set; }

}