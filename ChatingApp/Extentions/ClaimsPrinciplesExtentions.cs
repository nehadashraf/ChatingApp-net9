using System.Security.Claims;

namespace ChatingApp.Extentions
{
    public static class ClaimsPrinciplesExtentions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (username == null)
                throw new Exception( "No username found in token");
            return username;
        }

    }
}
