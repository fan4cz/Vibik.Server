using System.Text.Json.Serialization;

namespace Shared.Models.ExternalApi;

public class OpenWeatherResponse
{
    [JsonPropertyName("weather")] public List<WeatherDescription> Weather { get; set; } = [];

    [JsonPropertyName("main")] public TemperatureInfo? Main { get; set; }
}