using GHLearning.EasyCookie.Core.Accounts;

namespace GHLearning.EasyCookie.Application.Abstractions.Authentication;
public interface IAccountContext
{
	Guid NameIdentifier { get; }
	AccountEntity Account { get; }
}
