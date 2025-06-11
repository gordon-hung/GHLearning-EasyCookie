using CorrelationId;
using CorrelationId.DependencyInjection;
using GHLearning.EasyCookie.Application.Abstractions.Authentication;
using GHLearning.EasyCookie.Core.Accounts;
using GHLearning.EasyCookie.Core.WeatherForecasts;
using GHLearning.EasyCookie.Infrastructure.Accounts;
using GHLearning.EasyCookie.Infrastructure.Authentication;
using GHLearning.EasyCookie.Infrastructure.Correlations;
using GHLearning.EasyCookie.Infrastructure.RedisConnection;
using GHLearning.EasyCookie.Infrastructure.WeatherForecasts;
using GHLearning.EasyCookie.SharedKernel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace GHLearning.EasyCookie.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		Action<AccountOptions, IServiceProvider> accountOptions,
		Action<RedisOptions, IServiceProvider> redisOptions)
		=> services
		.AddOptions<AccountOptions>()
		.Configure(accountOptions)
		.Services
		.AddOptions<RedisOptions>()
		.Configure(redisOptions)
		.Services
		.AddSingleton(TimeProvider.System)
		.AddSingleton<ISequentialGuidGenerator, SequentialGuidGenerator>()
		.AddSingleton<RedisConnectionFactory>()
		.AddSingleton(sp => sp.GetRequiredService<RedisConnectionFactory>().Database)
		.AddAuthenticationInternal()
		.AddCorrelation()
		.AddWeatherForecastInfrastructure()
		.AddAccountInfrastructure();

	private static IServiceCollection AddAccountInfrastructure(this IServiceCollection services)
		=> services.AddTransient<IAccountRepository, AccountRepository>();

	private static IServiceCollection AddWeatherForecastInfrastructure(this IServiceCollection services)
		=> services.AddTransient<IWeatherForecastRepository, WeatherForecastRepository>();

	private static IServiceCollection AddAuthenticationInternal(
		this IServiceCollection services)
	{
		services
			.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				options.SlidingExpiration = true;
				options.ExpireTimeSpan = TimeSpan.FromHours(1);
				options.LoginPath = "/Api/Account/Login";
				options.LogoutPath = "/Api/Account/Logout";
				options.AccessDeniedPath = CookieAuthenticationDefaults.AccessDeniedPath;
			});

		services.AddHttpContextAccessor();
		services.AddScoped<IAccountContext, AccountContext>();
		services.AddSingleton<ICookieProvider, CookieProvider>();

		return services;
	}

	private static IServiceCollection AddCorrelation(this IServiceCollection services)
	{
		//Learn more about configuring CorrelationId at https://github.com/stevejgordon/CorrelationId/wiki
		services.AddCorrelationId<CustomCorrelationIdProvider>(options =>
		{
			options.AddToLoggingScope = true;
			options.LoggingScopeKey = CorrelationIdOptions.DefaultHeader;
		});

		return services;
	}
}
