using GHLearning.EasyCookie.Core.Accounts;
using GHLearning.EasyCookie.Infrastructure.Accounts;
using GHLearning.EasyCookie.SharedKernel;
using Microsoft.Extensions.Options;
using NSubstitute;
using StackExchange.Redis;

namespace GHLearning.EasyCookie.InfrastructureTests.Accounts;
public class AccountRepositoryTests
{
	[Fact]
	public async Task AuthenticationAsync_ReturnsAccount_WhenCredentialsAreValid()
	{
		// Arrange
		var accountOptions = Options.Create(new AccountOptions
		{
			Accounts =
			[
				new AccountEntity { Account = "testuser", Password = "password" }
			]
		});
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
		var fakeDatabase = Substitute.For<IDatabase>();
		var repository = new AccountRepository(
			accountOptions,
			fakeSequentialGuidGenerator,
			fakeDatabase);

		// Act
		var result = await repository.AuthenticationAsync("testuser", "password");

		// Assert
		Assert.NotNull(result);
		Assert.Equal("testuser", result?.Account);
		Assert.Equal("password", result?.Password);
	}

	[Fact]
	public async Task AuthenticationAsync_ReturnsNull_WhenCredentialsAreInvalid()
	{
		// Arrange
		var accountOptions = Options.Create(new AccountOptions
		{
			Accounts =
			[
				new AccountEntity { Account = "testuser", Password = "password" }
			]
		});
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
		var fakeDatabase = Substitute.For<IDatabase>();
		var repository = new AccountRepository(
			accountOptions,
			fakeSequentialGuidGenerator,
			fakeDatabase);
		// Act
		var result = await repository.AuthenticationAsync("testuser", "wrongpassword");
		// Assert
		Assert.Null(result);
	}
	[Fact]
	public async Task GetAccountAsync_ReturnsAccount_WhenAccountExists()
	{
		// Arrange
		var accountOptions = Options.Create(new AccountOptions
		{
			Accounts =
			[
				new AccountEntity { Account = "testuser", Password = "password" }
			]
		});
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
		var fakeDatabase = Substitute.For<IDatabase>();
		var repository = new AccountRepository(
			accountOptions,
			fakeSequentialGuidGenerator,
			fakeDatabase);
		// Act
		var result = await repository.GetAccountAsync("testuser");
		// Assert
		Assert.NotNull(result);
		Assert.Equal("testuser", result?.Account);
		Assert.Equal("password", result?.Password);
	}
	[Fact]
	public async Task GetAccountAsync_ReturnsNull_WhenAccountDoesNotExist()
	{
		// Arrange
		var accountOptions = Options.Create(new AccountOptions
		{
			Accounts =
			[
				new AccountEntity { Account = "testuser", Password = "password" }
			]
		});
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
		var fakeDatabase = Substitute.For<IDatabase>();
		var repository = new AccountRepository(
			accountOptions,
			fakeSequentialGuidGenerator,
			fakeDatabase);
		// Act
		var result = await repository.GetAccountAsync("nonexistentuser");
		// Assert
		Assert.Null(result);
	}
	[Fact]
	public async Task GetByCookie_ReturnsAccount_WhenCookieExists()
	{
		// Arrange
		var accountOptions = Options.Create(new AccountOptions
		{
			Accounts =
			[
				new AccountEntity { Account = "testuser", Password = "password" }
			]
		});
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
		var fakeDatabase = Substitute.For<IDatabase>();
		var repository = new AccountRepository(
			accountOptions,
			fakeSequentialGuidGenerator,
			fakeDatabase);
		var nameIdentifier = Guid.NewGuid();
		var key = $"account:cookie:{nameIdentifier}";
		fakeDatabase.StringGetAsync(
			key: Arg.Is<RedisKey>(key),
			flags: Arg.Any<CommandFlags>())
			.Returns(Task.FromResult<RedisValue>("testuser"));

		// Act
		var result = await repository.GetByCookie(nameIdentifier);
		// Assert
		Assert.NotNull(result);
		Assert.Equal("testuser", result?.Account);
		Assert.Equal("password", result?.Password);
	}
	[Fact]
	public async Task GetByCookie_ReturnsNull_WhenCookieDoesNotExist()
	{
		// Arrange
		var accountOptions = Options.Create(new AccountOptions
		{
			Accounts =
			[
				new AccountEntity { Account = "testuser", Password = "password" }
			]
		});
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
		var fakeDatabase = Substitute.For<IDatabase>();
		var repository = new AccountRepository(
			accountOptions,
			fakeSequentialGuidGenerator,
			fakeDatabase);
		var nameIdentifier = Guid.NewGuid();
		var key = $"account:cookie:{nameIdentifier}";
		fakeDatabase.StringGetAsync(
			key: Arg.Is<RedisKey>(key),
			flags: Arg.Any<CommandFlags>())
			.Returns(Task.FromResult(RedisValue.Null));
		// Act
		var result = await repository.GetByCookie(nameIdentifier);
		// Assert
		Assert.Null(result);
	}
	[Fact]
	public async Task SetByCookie_SetsCookieAndReturnsNameIdentifier()
	{
		// Arrange
		var accountOptions = Options.Create(new AccountOptions
		{
			Accounts =
			[
				new AccountEntity { Account = "testuser", Password = "password" }
			]
		});
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
		var fakeDatabase = Substitute.For<IDatabase>();
		var repository = new AccountRepository(
			accountOptions,
			fakeSequentialGuidGenerator,
			fakeDatabase);
		var nameIdentifier = Guid.NewGuid();
		fakeSequentialGuidGenerator.NewId().Returns(nameIdentifier);
		var key = $"account:cookie:{nameIdentifier}";
		
		// Act
		var result = await repository.SetByCookie("testuser");
		
		// Assert
		Assert.Equal(nameIdentifier, result);
		await fakeDatabase.Received(1).StringSetAsync(
			key: Arg.Is<RedisKey>(key),
			value: Arg.Is<RedisValue>("testuser"),
			expiry: Arg.Is<TimeSpan>(t => t.TotalDays == 1),
			flags: Arg.Any<CommandFlags>());
	}
	[Fact]
	public async Task LogoutByCookie_DeletesCookie()
	{
		// Arrange
		var accountOptions = Options.Create(new AccountOptions
		{
			Accounts =
			[
				new AccountEntity { Account = "testuser", Password = "password" }
			]
		});
		var fakeSequentialGuidGenerator = Substitute.For<ISequentialGuidGenerator>();
		var fakeDatabase = Substitute.For<IDatabase>();
		var repository = new AccountRepository(
			accountOptions,
			fakeSequentialGuidGenerator,
			fakeDatabase);
		var nameIdentifier = Guid.NewGuid();
		
		// Act
		await repository.LogoutByCookie(nameIdentifier);
		
		// Assert
		await fakeDatabase.Received(1).KeyDeleteAsync(
			key: Arg.Is<RedisKey>($"account:cookie:{nameIdentifier}"),
			flags: Arg.Any<CommandFlags>());
	}
}
