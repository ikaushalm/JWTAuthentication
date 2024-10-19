using Azure;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding Identity  Endpoints
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;



}
    );

builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("devdb")));


builder.Services.AddAuthentication(x => x.DefaultAuthenticateScheme =
x.DefaultChallengeScheme =
x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(y =>
    {
        y.SaveToken = false;
        y.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!))

        };
    });




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage<>(options=> options.);

}


#region  Cors 

app.UseCors();
#endregion

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGroup("/api").
    MapIdentityApi<AppUser>();


app.MapPost("/api/SignUp", async (UserManager<AppUser> usrmgr, [FromBody] RegistrationModel regmodel) =>
{
    AppUser user = new AppUser
    {
        FullName = regmodel.fullName,
        Email = regmodel.Email,
        UserName = regmodel.Email

    };
    var response = await usrmgr.CreateAsync(user, regmodel.Password);

    if (response.Succeeded)
    {
        return Results.Ok(response);
    }
    else
    {
        return Results.BadRequest(response);
    }

});




app.MapPost("/api/SignIn", async (UserManager<AppUser> usrmgr, [FromBody] LoginModel lginmodel) =>
{

    var user = await usrmgr.FindByEmailAsync(lginmodel.Email);
    if (user != null && await usrmgr.CheckPasswordAsync(user, lginmodel.Password))
    {
        var signkey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!));

        var tokendecriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            new Claim[]
            {
            new Claim("userId", user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(
                signkey, SecurityAlgorithms.HmacSha256)
        };

        var tokenhandler = new JwtSecurityTokenHandler();
        var securityToken = tokenhandler.CreateToken(tokendecriptor);
        var token = tokenhandler.WriteToken(securityToken);
        return Results.Ok(new { token });


    }
    else
    {
        return Results.BadRequest(new { message = "Username or password is wrong" });
    }
});



app.Run();

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