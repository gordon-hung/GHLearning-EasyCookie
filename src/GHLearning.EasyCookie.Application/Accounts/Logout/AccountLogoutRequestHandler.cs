using GHLearning.EasyCookie.Application.Abstractions.Authentication;
using GHLearning.EasyCookie.Core.Accounts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GHLearning.EasyCookie.Application.Accounts.Logout;
internal class AccountLogoutRequestHandler(
	ILogger<AccountLogoutRequestHandler> logger,
	IAccountContext accountContext,
	IAccountRepository accountRepository) : IRequestHandler<AccountLogoutRequest>
{
	public Task Handle(AccountLogoutRequest request, CancellationToken cancellationToken)
	{
		try
		{
			return accountRepository.LogoutByCookie(accountContext.NameIdentifier, cancellationToken);
		}
		catch (UnauthorizedAccessException ex)
		{
			logger.LogWarning(ex, "User is not authenticated or session has expired.");
			return Task.CompletedTask;
		}
	}
}
