using Api.Application.Common.Results;
using Shared.Models.ExternalApi;

namespace Infrastructure.Interfaces;

public interface IWeatherApi
{
    Task<Result<WeatherInfo>> GetCurrentWeatherAsync(CancellationToken ct = default);
}