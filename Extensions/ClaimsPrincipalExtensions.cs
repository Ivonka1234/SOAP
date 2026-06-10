using System.Security.Claims;

namespace SOAP.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public static bool IsAdmin(this ClaimsPrincipal user) =>
            user.IsInRole("Admin");
    }
}
