using System;
using Microsoft.Extensions.Logging;

namespace Grumpy.Logging
{
    public static class LoggerExtensions
    {
        public static void LogWarning(this ILogger logger, Exception exception, string message)
        {
            logger.LogWarning(0, exception, message);
        }
    }
}
