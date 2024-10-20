using Azure;
using JWTAuthentication.Controllers;
using JWTAuthentication.Extensions;
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


//cleaning the whole code
builder.Services.AddSwaggerUI()
                .InjectDBContext(builder.Configuration)
                .AddAppConfig(builder.Configuration)
                .AddIdentityHandlersAndStores()
                .ConfigureIdentityOptions()
                //injecting dbcontext
                .AddIdentityAuth(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureSwaggerExplorer()
    .ConfigureCors(builder.Configuration)
    .AddIdentityAuthMiddleware();



app.MapControllers();

app.MapGroup("/api").
    MapIdentityApi<AppUser>();

app.MapGroup("/api").
    MapIdentiyUserEndpoints().
    MapAccountEndpoints().
    MapAuthorizationDemoEndpoints();


app.Run();

