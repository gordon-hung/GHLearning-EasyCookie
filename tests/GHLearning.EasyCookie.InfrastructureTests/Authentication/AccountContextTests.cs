using System.Security.Claims;
using GHLearning.EasyCookie.Core.Accounts;
using GHLearning.EasyCookie.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;

namespace GHLearning.EasyCookie.InfrastructureTests.Authentication;
public class AccountContextTests
{
	[Fact]
	public void NameIdentifier_ShouldReturnExpectedValue_WhenSet()
	{
		// Arrange
		var expectedId = Guid.NewGuid();
		var httpContextAccessor = new HttpContextAccessor
		{
			HttpContext = new DefaultHttpContext()
		};
		httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
		{
			new Claim(ClaimTypes.NameIdentifier, expectedId.ToString("N"))
		}));
		var fakeAccountRepository = NSubstitute.Substitute.For<IAccountRepository>();

		// Act
		var accountContext = new AccountContext(httpContextAccessor, fakeAccountRepository);
		var result = accountContext.NameIdentifier;
		// Assert
		Assert.Equal(expectedId, result);
	}
	[Fact]
	public void NameIdentifier_ShouldThrowUnauthorizedAccessException_WhenUserIsNotAuthenticated()
	{
		// Arrange
		var httpContextAccessor = new HttpContextAccessor
		{
			HttpContext = new DefaultHttpContext()
		};
		httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
		var fakeAccountRepository = NSubstitute.Substitute.For<IAccountRepository>();
		// Act & Assert
		var accountContext = new AccountContext(httpContextAccessor, fakeAccountRepository);
		Assert.Throws<UnauthorizedAccessException>(() => _ = accountContext.NameIdentifier);
	}
}
