using System.Security.Claims;
using GHLearning.EasyCookie.Core.Accounts;

namespace GHLearning.EasyCookie.Application.Abstractions.Authentication;
public interface ICookieProvider
{
	ClaimsIdentity Create(Guid nameIdentifier, AccountEntity account);
}
