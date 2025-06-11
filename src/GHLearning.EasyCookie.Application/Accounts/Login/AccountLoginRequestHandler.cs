using GHLearning.EasyCookie.Application.Abstractions.Authentication;
using GHLearning.EasyCookie.Core.Accounts;
using MediatR;

namespace GHLearning.EasyCookie.Application.Accounts.Login;
internal class AccountLoginRequestHandler(
	IAccountRepository accountRepository,
	ICookieProvider cookieProvider) : IRequestHandler<AccountLoginRequest, AccountLoginResponse>
{
	public async Task<AccountLoginResponse> Handle(AccountLoginRequest request, CancellationToken cancellationToken)
	{
		var account = await accountRepository.AuthenticationAsync(request.Account, request.Password, cancellationToken).ConfigureAwait(false)
			?? throw new UnauthorizedAccessException("Invalid account or password.");

		var nameIdentifier = await accountRepository.SetByCookie(request.Account, cancellationToken).ConfigureAwait(false);

		var claimsIdentity = cookieProvider.Create(nameIdentifier, account);

		return new AccountLoginResponse(claimsIdentity);
	}
}
