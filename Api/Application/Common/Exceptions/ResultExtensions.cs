using Api.Application.Common.Results;

namespace Api.Application.Common.Exceptions;

public static class ResultExtensions
{
    public static T EnsureSuccess<T>(this Result<T> result)
    {
        if (!result.IsSuccess)
        {
            throw new ApiException(
                StatusCodes.Status503ServiceUnavailable,
                result.Error?.Message ?? "External service unavailable"
            );
        }

        return result.Value!;
    }
}