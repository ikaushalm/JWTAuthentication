using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace JWTAuthentication.Controllers
{
    public  static class AuthorisationDemoEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorizationDemoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/adminOnly", adminOnly);

            app.MapGet("/adminOrteacher", [Authorize(Roles = "admin,Teacher")] () =>
            { return "admin Or teacher"; });

            app.MapGet("/LibraryMembersOnly", [Authorize(Policy = "HasLibraryId")] () =>
            { return "Library members only"; });

            app.MapGet("ApplyForMaternityLeave", [Authorize(Roles = "Teacher", Policy = "FemalesOnly")] () =>
            { return "Applied for maternity leave."; });

            app.MapGet("/Under10sAndFemale",
            [Authorize(Policy = "Under10")]
            [Authorize(Policy = "FemalesOnly")] () =>
            { return "Under 10 And Female"; });

            return app;
        }

        [Authorize(Roles = "admin")]
        private static string adminOnly()
        { return "admin Only"; }
    }


}
