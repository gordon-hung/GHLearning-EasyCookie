using System.Security.Claims;
using GHLearning.EasyCookie.Application.Abstractions.Authentication;
using GHLearning.EasyCookie.Application.Accounts.Login;
using GHLearning.EasyCookie.Core.Accounts;
using Microsoft.AspNetCore.Authentication.Cookies;
using NSubstitute;

namespace GHLearning.EasyCookie.ApplicationTests.Accounts.Login;

public class AccountLoginRequestHandlerTests
{
	[Fact]
	public async Task Handle_ValidRequest_ReturnsSuccessResult()
	{
		// Arrange
		var fakeAccountRepository = Substitute.For<IAccountRepository>();
		var fakeCookieProvider = Substitute.For<ICookieProvider>();
		var handler = new AccountLoginRequestHandler(
			fakeAccountRepository,
			fakeCookieProvider);
		var request = new AccountLoginRequest(Account: "testuser", Password: "password123");

		var account = new AccountEntity
		{
			Account = "testuser",
			Password = "password123",
			Name = "testname"
		};
		_ = fakeAccountRepository.AuthenticationAsync(
			account: Arg.Is(request.Account),
			password: Arg.Is(request.Password),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(account);

		var nameIdentifier = Guid.NewGuid();
		_ = fakeAccountRepository.SetByCookie(
			Arg.Is(request.Account),
			Arg.Any<CancellationToken>())
			.Returns(nameIdentifier);

		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier,nameIdentifier.ToString("N")),
			new(ClaimTypes.Name, account.Name)
		};
		var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		_ = fakeCookieProvider.Create(
			nameIdentifier: Arg.Is(nameIdentifier),
			account: Arg.Any<AccountEntity>())
			.Returns(claimsIdentity);

		// Act
		var result = await handler.Handle(request, CancellationToken.None);
		// Assert
		Assert.NotNull(result);
		Assert.IsType<AccountLoginResponse>(result);
		Assert.Equal(claimsIdentity, result.ClaimsIdentity);
	}

	[Fact]
	public async Task Handle_InvalidAccount_ThrowsUnauthorizedAccessException()
	{
		// Arrange
		var fakeAccountRepository = Substitute.For<IAccountRepository>();
		var fakeCookieProvider = Substitute.For<ICookieProvider>();
		var handler = new AccountLoginRequestHandler(
			fakeAccountRepository,
			fakeCookieProvider);
		var request = new AccountLoginRequest(Account: "invaliduser", Password: "wrongpassword");
		_ = fakeAccountRepository.AuthenticationAsync(
			account: Arg.Is(request.Account),
			password: Arg.Is(request.Password),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<AccountEntity?>(null));
		// Act & Assert
		await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(request, CancellationToken.None));
	}
}
