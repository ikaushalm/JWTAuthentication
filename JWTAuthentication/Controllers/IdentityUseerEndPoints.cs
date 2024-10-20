using JWTAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Controllers
{
    public static class IdentityUseerEndPoints
    {

        public static IEndpointRouteBuilder MapIdentiyUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("SignUp", CreateUser);

            app.MapPost("SignIn", SignIn);

            return app;

        }

        [AllowAnonymous]
        private static async Task<IResult> CreateUser(UserManager<AppUser> usrmgr, [FromBody] RegistrationModel regmodel)
        {
            AppUser user = new AppUser
            {
                FullName = regmodel.fullName,
                Email = regmodel.Email,
                UserName = regmodel.Email,
                Gender = regmodel.Gender,
                DOB=DateOnly.FromDateTime(DateTime.Now.AddYears(-regmodel.Age)),
                LibraryID = regmodel.LibraryID
            };

            var response = await usrmgr.CreateAsync(user, regmodel.Password);
            await usrmgr.AddToRoleAsync(user, regmodel.Role);

            if (response.Succeeded)
            {
                return Results.Ok(response);
            }
            else
            {
                return Results.BadRequest(response);
            }

        }



        [AllowAnonymous]
        private static async Task<IResult> SignIn(UserManager<AppUser> usrmgr, [FromBody] LoginModel lginmodel, IOptions<AppSettings> appsettings)
        {
            var user = await usrmgr.FindByEmailAsync(lginmodel.Email);
            if (user != null && await usrmgr.CheckPasswordAsync(user, lginmodel.Password))
            {
                var signkey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(appsettings.Value.JWTSecret));

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

        }
    }
}
