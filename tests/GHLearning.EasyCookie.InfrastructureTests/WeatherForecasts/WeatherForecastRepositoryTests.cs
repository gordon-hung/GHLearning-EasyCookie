using GHLearning.EasyCookie.Core.WeatherForecasts;
using GHLearning.EasyCookie.Infrastructure.WeatherForecasts;

namespace GHLearning.EasyCookie.InfrastructureTests.WeatherForecasts;
public class WeatherForecastRepositoryTests
{
	[Fact]
	public async Task QueryAsync_ReturnsExpectedNumberOfForecasts()
	{
		// Arrange
		var repository = new WeatherForecastRepository();
		int count = 5;
		// Act
		var forecasts = await repository.QueryAsync(count).ToListAsync();
		// Assert
		Assert.Equal(count, forecasts.Count);
		foreach (var forecast in forecasts)
		{
			Assert.NotEqual(default, forecast.Date);
			Assert.InRange(forecast.TemperatureC, -20, 55);
			Assert.Contains(forecast.Summary, WeatherForecastSummary.Summaries);
		}
	}
	[Fact]
	public async Task QueryAsync_ReturnsEmpty_WhenCountIsZero()
	{
		// Arrange
		var repository = new WeatherForecastRepository();
		int count = 0;
		// Act
		var forecasts = await repository.QueryAsync(count).ToListAsync();
		// Assert
		Assert.Empty(forecasts);
	}
}
