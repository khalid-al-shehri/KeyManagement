using System.Net;

namespace Application.Common.Shared;

public class Result<T>
{

    internal Result(T data, MetaData metaData)
    {
        Data = data;
        MetaData = metaData;
    }

    public  T Data { get; set; }
    public MetaData MetaData {get; set;}

    public static Result<T> Success(T data)
    {
        MetaData metaData = new MetaData{
            Succeeded = true,
            Errors = null,
            StatusCode = HttpStatusCode.OK
        };

        return new Result<T>(data, metaData);
    }

    public static Result<T> Unauthorized(string msg)
    {
        MetaData metaData = new MetaData{
            Succeeded = false,
            Errors = new string[] { msg },
            StatusCode = HttpStatusCode.Unauthorized
        };

        return new Result<T>(default(T), metaData);
    }
    public static Result<T> NotFound(string msg)
    {
        MetaData metaData = new MetaData{
            Succeeded = false,
            Errors = new string[] { msg },
            StatusCode = HttpStatusCode.NotFound
        };

        return new Result<T>(default(T), metaData);
    }

    public static Result<T> Failure(IEnumerable<string> errors)
    {
        MetaData metaData = new MetaData{
            Succeeded = false,
            Errors = errors.ToArray(),
            StatusCode = HttpStatusCode.BadRequest
        };

        return new Result<T>(default(T), metaData);
    }

    public static Result<T> Failure(string error)
    {
        MetaData metaData = new MetaData{
            Succeeded = false,
            Errors = new string[] { error },
            StatusCode = HttpStatusCode.BadRequest
        };

        return new Result<T>(default(T), metaData);
    }
}

public record MetaData
{
    public bool Succeeded { get; set; }

    public string[] Errors { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}

