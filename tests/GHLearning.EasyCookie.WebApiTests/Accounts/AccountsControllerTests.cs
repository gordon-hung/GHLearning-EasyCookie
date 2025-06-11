using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Principal;
using GHLearning.EasyCookie.Application.Accounts.Login;
using GHLearning.EasyCookie.Application.Accounts.Logout;
using GHLearning.EasyCookie.WebApi.Controllers.Accounts.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace GHLearning.EasyCookie.WebApiTests.Accounts;
public class AccountsControllerTests
{
	[Fact]
	public async Task AccountLogin_ShouldReturnOk_WhenCredentialIsValid()
	{
		var factory = new CustomWebApplicationFactory(builder =>
		{
			var fakeSender = Substitute.For<ISender>();
			var claims = new List<Claim>
			{
				new(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString("N")),
				new(ClaimTypes.Name, "validUser")
			};
			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			_ = fakeSender.Send(Arg.Is<AccountLoginRequest>(
				comparer => comparer.Account == "validUser" &&
				comparer.Password == "validPassword"),
				Arg.Any<CancellationToken>())
			.Returns(new AccountLoginResponse(claimsIdentity));

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeSender));
		});
		var client = factory.CreateClient();
		var login = new AccountLoginViewModel(Account: "validUser", Password: "validPassword");

		var response = await client.PostAsJsonAsync("/api/accounts/login", login);

		response.EnsureSuccessStatusCode();
	}

	[Fact]
	public async Task AccountLogin_ShouldReturnUnauthorized_WhenCredentialIsInvalid()
	{
		var factory = new CustomWebApplicationFactory(builder =>
		{
			var fakeSender = Substitute.For<ISender>();
			_ = fakeSender.Send(Arg.Is<AccountLoginRequest>(
				comparer => comparer.Account == "invalidUser" &&
				comparer.Password == "invalidPassword"),
				Arg.Any<CancellationToken>())
			.Throws(new UnauthorizedAccessException("Invalid credentials."));
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeSender));
		});
		var client = factory.CreateClient();
		var login = new AccountLoginViewModel(Account: "invalidUser", Password: "invalidPassword");
		var response = await client.PostAsJsonAsync("/api/accounts/login", login);
		Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
		var content = await response.Content.ReadAsStringAsync();
		Assert.Contains("認證失敗，請檢查帳號或密碼。", content);
	}

	[Fact]
	public async Task AccountLogout_ShouldReturnOk()
	{
		var fakeSender = Substitute.For<ISender>();

		var factory = new CustomWebApplicationFactory(builder =>
		{


			var claims = new List<Claim>
			{
				new(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString("N")),
				new(ClaimTypes.Name, "validUser")
			};
			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			_ = fakeSender.Send(Arg.Is<AccountLoginRequest>(
				comparer => comparer.Account == "validUser" &&
				comparer.Password == "validPassword"),
				Arg.Any<CancellationToken>())
			.Returns(new AccountLoginResponse(claimsIdentity));

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeSender));
		});
		var client = factory.CreateClient();
		var login = new AccountLoginViewModel(Account: "validUser", Password: "validPassword");
		_ = await client.PostAsJsonAsync("/api/accounts/login", login);
		var response = await client.DeleteAsync("/api/accounts/logout");
		response.EnsureSuccessStatusCode();

		_ = fakeSender
			.Received(1)
			.Send(
			request: Arg.Any<AccountLogoutRequest>(),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task AccountLogout_ShouldReturnUnauthorized_WhenNotFound()
	{
		var factory = new CustomWebApplicationFactory(builder =>
		{
			var fakeSender = Substitute.For<ISender>();
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeSender));
		});
		var client = factory.CreateClient();
		var response = await client.DeleteAsync("/api/accounts/logout");
		Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
	}
}
