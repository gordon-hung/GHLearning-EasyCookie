using System.Security.Claims;

namespace GHLearning.EasyCookie.Application.Accounts.Login;
public record AccountLoginResponse(
	ClaimsIdentity ClaimsIdentity);
