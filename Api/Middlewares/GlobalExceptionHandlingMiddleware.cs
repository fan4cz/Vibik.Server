﻿using System.Text.Json;
using Api.Application.Common.Exceptions;

namespace Api.Middlewares;

public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ApiException e)
        {
            logger.LogWarning("API Error: {Code} - {EMessage}", e.Code, e.Message);            
            await HandleApiExceptionAsync(context, e);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception");
            await HandleUnknownErrorAsync(context);
        }
    }

    private static async Task HandleUnknownErrorAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var result = new
        {
            code = "internal_server_error",
            message = "An internal server error occurred"
        };
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }

    private static async Task HandleApiExceptionAsync(HttpContext context, ApiException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception.Code switch
        {
            "not_found" => StatusCodes.Status404NotFound,
            "validation" => StatusCodes.Status400BadRequest,
            "unauthorized" => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status400BadRequest
        };

        var result = new
        {
            code = exception.Code,
            message = exception.Message
        };
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
}