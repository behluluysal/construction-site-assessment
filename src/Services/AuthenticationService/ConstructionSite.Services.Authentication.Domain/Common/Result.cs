namespace ConstructionSite.Services.Authentication.Domain.Common;

public class Result
{
    public bool Succeeded { get; private set; }
    public List<string> Errors { get; private set; } = [];

    protected Result(bool succeeded, List<string>? errors = null)
    {
        Succeeded = succeeded;
        Errors = errors ?? [];
    }

    public static Result Success() => new(true);
    public static Result Failure(params string[] errors) => new(false, [.. errors]);
}

public class Result<T>
{
    public bool Succeeded { get; }
    public T Data { get; }
    public string[] Errors { get; }

    private Result(bool succeeded, T data, string[] errors)
    {
        Succeeded = succeeded;
        Data = data;
        Errors = errors;
    }

    public static Result<T> SuccessResult(T data) => new(true, data, []);
    public static Result<T> FailureResult(params string[] errors) => new(false, default!, errors);
}
