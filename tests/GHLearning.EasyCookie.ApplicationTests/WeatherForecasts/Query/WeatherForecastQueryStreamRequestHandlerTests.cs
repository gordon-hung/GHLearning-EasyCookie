using GHLearning.EasyCookie.Application.WeatherForecasts.Query;
using GHLearning.EasyCookie.Core.WeatherForecasts;
using NSubstitute;

namespace GHLearning.EasyCookie.ApplicationTests.WeatherForecasts.Query;
public class WeatherForecastQueryStreamRequestHandlerTests
{
	[Fact]
	public async Task Handle_ValidRequest_ReturnsWeatherForecasts()
	{
		// Arrange
		var fakeWeatherForecastRepository = Substitute.For<IWeatherForecastRepository>();
		var handler = new WeatherForecastQueryStreamRequestHandler(
			fakeWeatherForecastRepository);
		var request = new WeatherForecastQueryStreamRequest(Count: 5);
		_ = fakeWeatherForecastRepository
			.QueryAsync(Arg.Is(request.Count), Arg.Any<CancellationToken>())
			.Returns(Enumerable.Range(1, request.Count).Select(index => new WeatherForecastEntity
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = WeatherForecastSummary.Summaries[Random.Shared.Next(WeatherForecastSummary.Summaries.Length)]
			}).ToAsyncEnumerable());
		// Act
		var result = await handler.Handle(request, CancellationToken.None).ToArrayAsync();
		// Assert
		Assert.NotNull(result);
		Assert.Equal(request.Count, result.Length);
	}
}
