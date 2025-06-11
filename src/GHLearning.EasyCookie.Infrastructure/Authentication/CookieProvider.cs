using System.Security.Claims;
using GHLearning.EasyCookie.Application.Abstractions.Authentication;
using GHLearning.EasyCookie.Core.Accounts;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GHLearning.EasyCookie.Infrastructure.Authentication;
internal class CookieProvider : ICookieProvider
{
	public ClaimsIdentity Create(Guid nameIdentifier, AccountEntity account)
	{
		var claims = new List<Claim>
			{
				new(ClaimTypes.NameIdentifier,nameIdentifier.ToString("N")),
				new(ClaimTypes.Name, account.Name)
			};
		return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
	}
}
