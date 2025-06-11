using Microsoft.Extensions.Options;
using GHLearning.EasyCookie.Infrastructure.RedisConnection;

namespace GHLearning.EasyCookie.InfrastructureTests.RedisConnection;

public class RedisConnectionFactoryTests
{
	[Fact]
	public void Constructor_ShouldInitializeDatabase()
	{
		// Arrange
		var options = new FakeOptions("localhost:6379"); // 須有本地 Redis，否則請用 mock
		using var factory = new RedisConnectionFactory(options);

		// Act & Assert
		Assert.NotNull(factory.Database);
		Assert.True(factory.Database.IsConnected(default));
	}

	[Fact]
	public void Dispose_ShouldNotThrow()
	{
		// Arrange
		var options = new FakeOptions("localhost:6379");
		var factory = new RedisConnectionFactory(options);

		// Act & Assert
		var ex = Record.Exception(() => factory.Dispose());
		Assert.Null(ex);
	}

	private class FakeOptions : IOptions<RedisOptions>
	{
		public RedisOptions Value { get; }

		public FakeOptions(string connectionString)
		{
			Value = new RedisOptions { ConnectionString = connectionString };
		}
	}
}
