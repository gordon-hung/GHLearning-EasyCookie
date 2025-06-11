using System.Security.Claims;

namespace GHLearning.EasyCookie.Infrastructure.Authentication;
internal static class ClaimsPrincipalExtensions
{
	public static string GetNameIdentifier(this ClaimsPrincipal? principal)
		=> principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
		?? throw new UnauthorizedAccessException("User account is unavailable");
}
