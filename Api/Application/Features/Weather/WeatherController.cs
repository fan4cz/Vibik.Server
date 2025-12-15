using Api.Application.Common.Exceptions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Features.Weather;

[ApiController]
[Route("api/[controller]")]
public class WeatherController(IWeatherApi weatherService) : ControllerBase
{
    /// <summary>
    /// Get current weather
    /// </summary>
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentWeather(CancellationToken cancellationToken)
    {
        var weather = (await weatherService
            .GetCurrentWeatherAsync(cancellationToken)).EnsureSuccess();
        
        return Ok(weather);
    }
}