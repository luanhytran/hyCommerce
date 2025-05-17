namespace hyCommerce.Domain.Common;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    private Result(bool isSuccess, T? data = default, string? message = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
    }

    public static Result<T> Success(T? data = default, string? message = null) => new Result<T>(true, data, message);
    public static Result<T> Failure(T? data = default, string? message = null) => new Result<T>(false, data, message);
}

public class Result
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    
    private Result(bool isSuccess, string? message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static Result Success(string message) => new Result(true, message);
    public static Result Failure(string message) => new Result(false, message);
}