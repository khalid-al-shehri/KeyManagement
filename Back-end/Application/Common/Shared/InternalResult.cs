using System;

namespace Application.Common.Shared;

public class InternalResult<T>
{

    private InternalResult(T data, IEnumerable<string> errors)
    {
        Data = data;
        Succeeded = errors is null || errors.Count() == 0;
        Errors = errors.ToArray();
    }

    public T Data { get; }

    public bool Succeeded { get; }

    public string[] Errors { get; }

    public static InternalResult<T> Success(T data) => new InternalResult<T>(data, Array.Empty<string>());

    public static InternalResult<T> Failure(IEnumerable<string> errors) => new InternalResult<T>(default(T), errors);

    public static InternalResult<T> Failure(string error) => new InternalResult<T>(default(T), new string[] { error });
}