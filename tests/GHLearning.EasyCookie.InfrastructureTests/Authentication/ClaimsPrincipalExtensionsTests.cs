using System.Security.Claims;
using GHLearning.EasyCookie.Infrastructure.Authentication;

namespace GHLearning.EasyCookie.InfrastructureTests.Authentication;
public class ClaimsPrincipalExtensionsTests
{
	[Fact]
	public void GetNameIdentifier_ShouldReturnValue_WhenClaimExists()
	{
		// Arrange
		var expectedId = Guid.NewGuid().ToString("N");
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, expectedId)
		};
		var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
		// Act
		var result = principal.GetNameIdentifier();
		// Assert
		Assert.Equal(expectedId, result);
	}

	[Fact]
	public void GetNameIdentifier_ShouldThrowException_WhenClaimDoesNotExist()
	{
		// Arrange
		var principal = new ClaimsPrincipal(new ClaimsIdentity());
		// Act & Assert
		Assert.Throws<UnauthorizedAccessException>(() => principal.GetNameIdentifier());
	}
}
