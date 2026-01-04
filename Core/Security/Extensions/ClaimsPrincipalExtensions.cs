using System.Security.Claims;

namespace Core.Security.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static List<string>? Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            List<string>? result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result;
        }

        public static List<string>? ClaimRoles(this ClaimsPrincipal claimsPrincipal) => 
            claimsPrincipal?.Claims(ClaimTypes.Role);

        public static int GetUserId(this ClaimsPrincipal claimsPrincipal) =>
            Convert.ToInt32(claimsPrincipal?.Claims(ClaimTypes.NameIdentifier)?.FirstOrDefault());

        public static string? GetUserEmail(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal?.Claims(ClaimTypes.Email)?.FirstOrDefault();

        public static string? GetUserName(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal?.Claims(ClaimTypes.Name)?.FirstOrDefault();
    }
}
