namespace GHLearning.EasyCookie.Core.Accounts;
public interface IAccountRepository
{
	Task<AccountEntity?> GetAccountAsync(string account, CancellationToken cancellationToken = default);
	Task<AccountEntity?> AuthenticationAsync(string account, string password, CancellationToken cancellationToken = default);
	Task<Guid> SetByCookie(string account, CancellationToken cancellationToken = default);
	Task<AccountEntity?> GetByCookie(Guid nameIdentifier, CancellationToken cancellationToken = default);
	Task LogoutByCookie(Guid nameIdentifier, CancellationToken cancellationToken = default);
}
