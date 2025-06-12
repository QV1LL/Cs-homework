namespace Warehouse.Common;

internal class Result
{
    internal bool IsSuccess { get; }
    internal string? Error { get; }

    internal bool IsFailure => !IsSuccess;

    protected Result(bool success, string? error)
    {
        IsSuccess = success;
        Error = error;
    }

    internal static Result Success() => new(true, null);
    internal static Result Failure(string error) => new(false, error);
}

internal class Result<T> : Result
{
    internal T? Value { get; }

    protected Result(T? value, bool success, string? error) : base(success, error)
    {
        Value = value;
    }

    internal static Result<T> Success(T value) => new(value, true, null);
    internal static new Result<T> Failure(string error) => new(default, false, error);
}
