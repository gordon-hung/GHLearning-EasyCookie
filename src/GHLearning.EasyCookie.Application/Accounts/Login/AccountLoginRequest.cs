using MediatR;

namespace GHLearning.EasyCookie.Application.Accounts.Login;
public record AccountLoginRequest(
	string Account,
	string Password) : IRequest<AccountLoginResponse>;
