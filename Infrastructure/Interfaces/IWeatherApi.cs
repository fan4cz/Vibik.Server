using Shared.Models.ExternalApi;

namespace Infrastructure.Interfaces;

public interface IWeatherApi
{
    Task<WeatherInfo> GetCurrentWeatherAsync(CancellationToken ct = default);
}