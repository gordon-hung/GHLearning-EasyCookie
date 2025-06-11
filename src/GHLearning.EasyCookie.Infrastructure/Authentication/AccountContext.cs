using GHLearning.EasyCookie.Application.Abstractions.Authentication;
using GHLearning.EasyCookie.Core.Accounts;
using Microsoft.AspNetCore.Http;

namespace GHLearning.EasyCookie.Infrastructure.Authentication;
internal class AccountContext(
	IHttpContextAccessor httpContextAccessor,
	IAccountRepository accountRepository) : IAccountContext
{
	public AccountEntity Account
	{
		get
		{
			return GetAsncy().GetAwaiter().GetResult();
		}
	}

	public Guid NameIdentifier
	{
		get
		{
			var id = httpContextAccessor.HttpContext?.User.GetNameIdentifier();

			return string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var nameIdentifier)
				? throw new UnauthorizedAccessException("User is not authenticated or session has expired.")
				: nameIdentifier;
		}
	}

	private async Task<AccountEntity> GetAsncy()
	{
		return await accountRepository.GetByCookie(NameIdentifier).ConfigureAwait(false)
			?? throw new ApplicationException("Account is unavailable");
	}
}
