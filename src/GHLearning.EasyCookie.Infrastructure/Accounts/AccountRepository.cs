using GHLearning.EasyCookie.Core.Accounts;
using GHLearning.EasyCookie.SharedKernel;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace GHLearning.EasyCookie.Infrastructure.Accounts;

internal class AccountRepository(
	IOptions<AccountOptions> accountOptions,
	ISequentialGuidGenerator sequentialGuidGenerator,
	IDatabase database) : IAccountRepository
{
	public Task<AccountEntity?> AuthenticationAsync(string account, string password, CancellationToken cancellationToken = default)
		=> accountOptions.Value.Accounts
		.Where(a => a.Account == account && a.Password == password)
		.ToAsyncEnumerable()
		.FirstOrDefaultAsync(cancellationToken)
		.AsTask();
	public Task<AccountEntity?> GetAccountAsync(string account, CancellationToken cancellationToken = default)
		=> accountOptions.Value.Accounts
		.Where(a => a.Account == account)
		.ToAsyncEnumerable()
		.FirstOrDefaultAsync(cancellationToken)
		.AsTask();
	public async Task<AccountEntity?> GetByCookie(Guid nameIdentifier, CancellationToken cancellationToken = default)
	{
		var key = $"account:cookie:{nameIdentifier}";

		var redisValue = await database.StringGetAsync(key, CommandFlags.PreferMaster).ConfigureAwait(false);

		return redisValue.IsNull
			? null
			: await GetAccountAsync(redisValue.ToString(), cancellationToken).ConfigureAwait(false);
	}

	public Task LogoutByCookie(Guid nameIdentifier, CancellationToken cancellationToken = default)
		=> _ = database.KeyDeleteAsync($"account:cookie:{nameIdentifier}", CommandFlags.PreferMaster);

	public async Task<Guid> SetByCookie(string account, CancellationToken cancellationToken = default)
	{
		var nameIdentifier = sequentialGuidGenerator.NewId();
		var key = $"account:cookie:{nameIdentifier}";

		await database.StringSetAsync(key, account, TimeSpan.FromDays(1)).ConfigureAwait(false);

		return nameIdentifier;
	}
}
