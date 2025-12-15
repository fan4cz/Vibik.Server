using Api.Application.Common.Results;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Models.Configs;
using Shared.Models.ExternalApi;

namespace Infrastructure.Api;

using System.Net.Http.Json;

public class WeatherService : IWeatherApi
{
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

    private readonly HttpClient httpClient;
    private readonly TimeSpan cacheDuration;
    private readonly SemaphoreSlim cacheLock = new(1, 1);
    private WeatherInfo? cached;
    private DateTimeOffset cacheExpiresAt;

    private readonly string apiKey;
    private readonly string latitude;
    private readonly string longitude;

    public WeatherService(HttpClient httpClient, IOptions<WeatherConfig> weatherConfig)
    {
        this.httpClient = httpClient;
        cacheDuration = TimeSpan.FromMinutes(5);

        var config = weatherConfig.Value;
        apiKey = config.ApiKey;
        latitude = config.Latitude;
        longitude = config.Longitude;
    }

    public async Task<Result<WeatherInfo>> GetCurrentWeatherAsync(CancellationToken ct = default)
    {
        if (cached != null && cacheExpiresAt > DateTimeOffset.UtcNow)
            return Result<WeatherInfo>.Ok(cached);

        await cacheLock.WaitAsync(ct);

        try
        {
            if (cached != null && cacheExpiresAt > DateTimeOffset.UtcNow)
                return Result<WeatherInfo>.Ok(cached);

            var info = await FetchFromApiAsync(ct);

            if (info.IsSuccess)
            {
                cached = info.Value!;
                cacheExpiresAt = DateTimeOffset.UtcNow.Add(cacheDuration);
            }


            return info;
        }
        finally
        {
            cacheLock.Release();
        }
    }

    private async Task<Result<WeatherInfo>> FetchFromApiAsync(CancellationToken ct)
    {
        if (string.IsNullOrEmpty(apiKey))
            return Result<WeatherInfo>.Fail(
                "weather.config",
                "API key is not configured for weather service.");

        try
        {
            var url = $"{BaseUrl}?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric&lang=ru";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.AcceptLanguage.ParseAdd("ru");

            using var response = await httpClient.SendAsync(request, ct);

            if (!response.IsSuccessStatusCode)
                return Result<WeatherInfo>.Fail(
                    "weather.http",
                    $"Weather service returned {(int)response.StatusCode}");

            var payload = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>(cancellationToken: ct);

            if (payload?.Weather is null || payload.Weather.Count == 0 || payload.Main is null)
                return Result<WeatherInfo>.Fail(
                    "weather.payload",
                    "Invalid weather data received");

            var w = payload.Weather[0];

            return Result<WeatherInfo>.Ok(new WeatherInfo
            {
                TemperatureCelsius = payload.Main.Temp,
                Condition = w.Main ?? "Unknown",
                Description = w.Description ?? string.Empty,
                RetrievedAt = DateTimeOffset.UtcNow
            });
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return Result<WeatherInfo>.Fail(
                "weather.exception",
                ex.Message);
        }
    }
}