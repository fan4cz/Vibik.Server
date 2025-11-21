namespace Api.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
{
    private readonly ILogger logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (!context.Response.HasStarted)
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            logger.LogError(ex,
                "Unhandled exception while processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            throw;
        }
        finally
        {
            var routeData = context.GetRouteData();
            var controller = routeData.Values["controller"]?.ToString();
            var action = routeData.Values["action"]?.ToString();

            logger.LogInformation(
                "[{Time:yyyy-MM-dd HH:mm:ss}] HTTP {Method} {Path} | {Controller}/{Action} | Status={StatusCode}",
                DateTime.UtcNow,
                context.Request.Method,
                context.Request.Path,
                controller ?? "(no controller)",
                action ?? "(no action)",
                context.Response.StatusCode);
        }
    }
}