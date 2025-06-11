using System.Security.Claims;
using GHLearning.EasyCookie.Core.Accounts;
using GHLearning.EasyCookie.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GHLearning.EasyCookie.InfrastructureTests.Authentication;
public class CookieProviderTests
{
	[Fact]
	public void Create_ShouldReturnClaimsIdentity_WithCorrectClaims()
	{
		// Arrange
		var provider = new CookieProvider();
		var userId = Guid.NewGuid();
		var account = new AccountEntity { Name = "TestUser" };

		// Act
		var identity = provider.Create(userId, account);

		// Assert
		Assert.NotNull(identity);
		Assert.Equal(CookieAuthenticationDefaults.AuthenticationScheme, identity.AuthenticationType);

		var nameIdentifierClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
		var nameClaim = identity.FindFirst(ClaimTypes.Name);

		Assert.NotNull(nameIdentifierClaim);
		Assert.Equal(userId.ToString("N"), nameIdentifierClaim.Value);

		Assert.NotNull(nameClaim);
		Assert.Equal("TestUser", nameClaim.Value);
	}
}
