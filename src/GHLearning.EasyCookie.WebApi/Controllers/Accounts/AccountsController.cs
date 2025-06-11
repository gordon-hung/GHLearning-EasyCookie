using System.Security.Claims;
using GHLearning.EasyCookie.Application.Accounts.Login;
using GHLearning.EasyCookie.Application.Accounts.Logout;
using GHLearning.EasyCookie.WebApi.Controllers.Accounts.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.EasyCookie.WebApi.Controllers.Accounts;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountsController(ILogger<AccountsController> logger) : ControllerBase
{
	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> AccountLoginAsync(
		[FromServices] ISender sender,
		[FromBody] AccountLoginViewModel source)
	{
		try
		{
			var response = await sender.Send(
				request: new AccountLoginRequest(
					source.Account,
					source.Password),
				cancellationToken: HttpContext.RequestAborted)
				.ConfigureAwait(false);

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(response.ClaimsIdentity),
				new AuthenticationProperties
				{
					IsPersistent = true,
					ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
				}).ConfigureAwait(false);

			return Ok();
		}
		catch (UnauthorizedAccessException ex)
		{
			logger.LogError(ex, "User login failed for account: {Account}", source.Account);

			return Unauthorized(new { message = "認證失敗，請檢查帳號或密碼。" });
		}
	}

	[HttpDelete("logout")]
	public async Task<IActionResult> AccountLogoutAsync(
		[FromServices] ISender sender)
	{
		await sender.Send(
			request: new AccountLogoutRequest(),
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		await HttpContext.SignOutAsync().ConfigureAwait(false);

		return Ok();
	}
}
