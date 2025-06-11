using GHLearning.EasyCookie.Application.Abstractions.Authentication;
using GHLearning.EasyCookie.Application.Accounts.Logout;
using GHLearning.EasyCookie.Core.Accounts;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace GHLearning.EasyCookie.ApplicationTests.Accounts.Logout;
public class AccountLogoutRequestHandlerTests
{
	[Fact]
	public async Task Handle_ValidRequest_ReturnsSuccessResult()
	{
		// Arrange
		var fakeLogger = NullLogger<AccountLogoutRequestHandler>.Instance;
		var fakeAccountContext = Substitute.For<IAccountContext>();
		var fakeAccountRepository = Substitute.For<IAccountRepository>();
		var handler = new AccountLogoutRequestHandler(
			fakeLogger,
			fakeAccountContext,
			fakeAccountRepository);
		var request = new AccountLogoutRequest();

		var nameIdentifier = Guid.NewGuid();
		fakeAccountContext.NameIdentifier.Returns(nameIdentifier);
		// Act
		await handler.Handle(request, CancellationToken.None);
		// Assert
		_ = fakeAccountRepository
			.Received(1)
			.LogoutByCookie(
			nameIdentifier: Arg.Is(nameIdentifier),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Handle_UnauthorizedAccessException()
	{
		// Arrange
		var fakeLogger = NullLogger<AccountLogoutRequestHandler>.Instance;
		var fakeAccountContext = Substitute.For<IAccountContext>();
		var fakeAccountRepository = Substitute.For<IAccountRepository>();
		var handler = new AccountLogoutRequestHandler(
			fakeLogger,
			fakeAccountContext,
			fakeAccountRepository);
		var request = new AccountLogoutRequest();
		fakeAccountContext.NameIdentifier.Throws(new UnauthorizedAccessException("User is not authenticated or session has expired."));
		// Act
		await handler.Handle(request, CancellationToken.None);
		// Assert
		_ = fakeAccountRepository
			.DidNotReceive()
			.LogoutByCookie(
			nameIdentifier: Arg.Any<Guid>(),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
