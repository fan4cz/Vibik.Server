namespace Api.Application.Common.Exceptions;

public class ApiException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}