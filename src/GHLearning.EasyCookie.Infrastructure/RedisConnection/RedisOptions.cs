namespace GHLearning.EasyCookie.Infrastructure.RedisConnection;
public record RedisOptions
{
	public string ConnectionString { get; set; } = "localhost";
}

