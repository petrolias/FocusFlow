namespace FocusFlow.Abstractions.Common
{
    public interface IResult
    {
        Exception? Exception { get; }
        bool IsSuccess { get; }
        string? Message { get; }
        int StatusCode { get; }
    }
}