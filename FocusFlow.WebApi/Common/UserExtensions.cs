using System.Security.Claims;

namespace FocusFlow.WebApi.Common
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new InvalidOperationException("User ID not found in claims.");
            return userId;
        }
    }
}