using System.Runtime.CompilerServices;

namespace FocusFlow.Abstractions.Common
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// Gets caller type and callermember name
        /// </summary>
        public static string Caller(this object callera, string message, 
            [CallerMemberName] string callerMemberName = "") =>            
            $"{callera.GetType().Name}.{callerMemberName}: {message}";
    }
}
