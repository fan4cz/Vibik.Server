using System.Text.Json.Serialization;

namespace Shared.Models.ExternalApi;

public class TemperatureInfo
{
    [JsonPropertyName("temp")] public double Temp { get; set; }
}