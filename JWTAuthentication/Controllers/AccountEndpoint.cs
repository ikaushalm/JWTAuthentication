using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JWTAuthentication.Controllers
{
    public static class AccountEndpoint
    {
        public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/UserProfile", GetUserProfile);

            return app;

        }

        [Authorize]
        private static async Task<IResult> GetUserProfile( UserManager<AppUser> usermanager,ClaimsPrincipal user)
        {
            string User_id = user.Claims.First(x => x.Type == "userId").Value;
            var userdetails = await usermanager.FindByIdAsync(User_id);

            return Results.Ok(
                new
                {
                    Name=userdetails?.FullName,
                    Email=userdetails?.Email

                });

        }
    }
}
