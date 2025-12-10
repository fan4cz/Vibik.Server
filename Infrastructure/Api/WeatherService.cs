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

    public async Task<WeatherInfo> GetCurrentWeatherAsync(CancellationToken ct = default)
    {
        if (cached != null && cacheExpiresAt > DateTimeOffset.UtcNow)
            return cached;

        await cacheLock.WaitAsync(ct);

        try
        {
            if (cached != null && cacheExpiresAt > DateTimeOffset.UtcNow)
                return cached;

            var info = await FetchFromApiAsync(ct);

            cached = info;
            cacheExpiresAt = DateTimeOffset.UtcNow.Add(cacheDuration);

            return info;
        }
        finally
        {
            cacheLock.Release();
        }
    }

    private async Task<WeatherInfo> FetchFromApiAsync(CancellationToken ct)
    {
        if (apiKey is null)
            throw new InvalidOperationException("API key is not configured for weather service.");

        var url = $"{BaseUrl}?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric&lang=ru";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.AcceptLanguage.ParseAdd("ru");

        using var response = await httpClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>(cancellationToken: ct);
        if (payload?.Weather is null || payload.Weather.Count == 0 || payload.Main is null)
            throw new InvalidOperationException("Weather response is missing required fields.");

        var w = payload.Weather[0];
        return new WeatherInfo
        {
            TemperatureCelsius = payload.Main.Temp,
            Condition = w.Main ?? "Unknown",
            Description = w.Description ?? string.Empty,
            RetrievedAt = DateTimeOffset.UtcNow
        };
    }
}