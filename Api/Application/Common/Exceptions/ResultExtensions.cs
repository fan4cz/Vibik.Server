using Api.Application.Common.Results;

namespace Api.Application.Common.Exceptions;

public static class ResultExtensions
{
    public static T EnsureSuccess<T>(this Result<T> result)
    {
        if (!result.IsSuccess)
        {
            var error = result.Error ?? new Error("unknown", "Unknown error");
            throw new ApiException(StatusCodes.Status500InternalServerError, error.Message);
        }

        return result.Value!;
    }
}