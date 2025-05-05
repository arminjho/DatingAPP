using System.Security.Claims;

namespace DatingWebApp.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var username=user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("Cannot get username from token");
            return username;    

        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
