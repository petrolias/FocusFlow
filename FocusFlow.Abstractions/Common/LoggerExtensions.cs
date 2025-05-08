using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FocusFlow.Abstractions.Common
{
    public static class LoggerExtensions
    {
        public static Result<T> FailureLog<T>(this ILogger logger, LogLevel logLevel = LogLevel.Error,  EventId? eventId = null,
            string? message = "", Exception? exception = null,
            [CallerMemberName] string callerMemberName = "")
        {
            eventId ??= (int)StatusCodes.Status500InternalServerError;
            message ??= exception?.Message ?? string.Empty;
            logger.Log(logLevel, eventId.Value, exception, message);
            return Result<T>.Failure($"{callerMemberName} {message}", exception);
        }
    }
}