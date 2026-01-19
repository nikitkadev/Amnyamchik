using Amnyam._1_Domain.Enums;

namespace Amnyam.Shared.Results;

public readonly struct BaseResult<T>(bool isSuccess, T? value, Error? error)
{
    public bool IsSuccess { get; } = isSuccess;
    public T? Value { get; } = value;
    public Error? Error { get; } = error;

    public static BaseResult<T> Success(T value) => new(true, value, null);
    public static BaseResult<T> Fail(Error error) => new(false, default, error);
}

public readonly struct BaseResult(bool isSuccess, string messageToClient, Error? error)
{
    public bool IsSuccess { get; } = isSuccess;
    public string ClientMessage { get; } = messageToClient;
    public Error? Error { get; } = error;

    public static BaseResult Success(string messageToClient) => new(true, messageToClient, null);
    public static BaseResult Fail(string messageToClient, Error error) => new(false, messageToClient, error);
}

public record Error(ErrorCodes ErrorCode, string Details);
