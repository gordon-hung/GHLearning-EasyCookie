using GHLearning.EasyCookie.Application.WeatherForecasts.Query;
using GHLearning.EasyCookie.WebApi.Controllers.WeatherForecasts.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.EasyCookie.WebApi.Controllers.WeatherForecasts;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WeatherForecastsController : ControllerBase
{
	[HttpGet(Name = "GetWeatherForecast")]
	public IAsyncEnumerable<WeatherForecastQueryResponse> QueryAsync(
		[FromServices] ISender sender,
		[FromQuery] WeatherForecastQueryViewModel source)
	=> sender.CreateStream(
		new WeatherForecastQueryStreamRequest(source.Count),
		cancellationToken: HttpContext.RequestAborted);
}
