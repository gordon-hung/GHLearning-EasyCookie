using GHLearning.EasyCookie.Core.Accounts;

namespace GHLearning.EasyCookie.Infrastructure.Accounts;
public record AccountOptions
{
	public IReadOnlyCollection<AccountEntity> Accounts { get; init; } = [];
}
