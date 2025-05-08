using System.Runtime.CompilerServices;

namespace FocusFlow.Abstractions.Common
{
    public class Result<T> : IResult
    {
        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }
        public string? Message { get; private set; }
        public Exception? Exception { get; private set; }
        public int StatusCode { get; private set; }

        public static Result<T> Success(T data, string? message = null, int statusCode = 200)
            => new Result<T> { IsSuccess = true, Value = data, Message = message, StatusCode = statusCode };

        public static Result<T> Failure(string message, Exception? exception = null, int statusCode = 500)
            => new Result<T>
            {
                IsSuccess = false,
                Message = message,
                Exception = exception,
                StatusCode = statusCode
            };

        public static Result<T> Failure(Exception? exception = null, int statusCode = 500)
            => new Result<T>
            {
                IsSuccess = false,
                Message = string.Empty,
                Exception = exception,
                StatusCode = statusCode
            };

        public static Result<T> From(IResult result)
           => new()
           {
               IsSuccess = result.IsSuccess,
               Message = result.Message,
               Exception = result.Exception,
               StatusCode = result.StatusCode
           };    
    }
}
