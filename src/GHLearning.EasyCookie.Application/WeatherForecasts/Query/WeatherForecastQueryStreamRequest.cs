using MediatR;

namespace GHLearning.EasyCookie.Application.WeatherForecasts.Query;
public record WeatherForecastQueryStreamRequest(
	int Count) : IStreamRequest<WeatherForecastQueryResponse>;