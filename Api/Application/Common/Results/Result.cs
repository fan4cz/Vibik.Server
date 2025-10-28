namespace Api.Application.Common.Results;

public readonly record struct Error(string Code, string Message);

public readonly record struct Result<T>(T? Value, Error? Error)
{
    public bool IsSuccess => Error is null;
    public static Result<T> Ok(T value) => new(value, null);
    public static Result<T> Fail(string code, string message) => new(default, new Error(code, message));
}

public readonly record struct Result
{
    public bool IsSuccess => Error is null;
    public Error? Error { get; }

    private Result(Error? error) => Error = error;

    public static Result Ok() => new(null);
    public static Result Fail(string code, string message) => new(new Error(code, message));
}