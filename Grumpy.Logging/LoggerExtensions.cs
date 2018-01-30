using System;
using Microsoft.Extensions.Logging;

namespace Grumpy.Logging
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILogger logger, Exception exception, string message, params object[] objects)
        {
            Log(logger, LogLevel.Debug, exception, message, objects);
        }

        public static void Debug(this ILogger logger, string message, params object[] objects)
        {
            Debug(logger, null, message, objects);
        }

        public static void Trace(this ILogger logger, Exception exception, string message, params object[] objects)
        {
            Log(logger, LogLevel.Trace, exception, message, objects);
        }

        public static void Trace(this ILogger logger, string message, params object[] objects)
        {
            Trace(logger, null, message, objects);
        }

        public static void Information(this ILogger logger, Exception exception, string message, params object[] objects)
        {
            Log(logger, LogLevel.Information, exception, message, objects);
        }

        public static void Information(this ILogger logger, string message, params object[] objects)
        {
            Information(logger, null, message, objects);
        }

        public static void Warning(this ILogger logger, Exception exception, string message, params object[] objects)
        {
            Log(logger, LogLevel.Warning, exception, message, objects);
        }

        public static void Warning(this ILogger logger, string message, params object[] objects)
        {
            Warning(logger, null, message, objects);
        }

        public static void Error(this ILogger logger, Exception exception, string message, params object[] objects)
        {
            Log(logger, LogLevel.Error, exception, message, objects);
        }

        public static void Error(this ILogger logger, string message, params object[] objects)
        {
            Error(logger, null, message, objects);
        }

        public static void Critical(this ILogger logger, Exception exception, string message, params object[] objects)
        {
            Log(logger, LogLevel.Critical, exception, message, objects);
        }

        public static void Critical(this ILogger logger, string message, params object[] objects)
        {
            Critical(logger, null, message, objects);
        }

        private static void Log(ILogger logger, LogLevel logLevel, Exception exception, string message, params object[] objects)
        {
            logger.Log(logLevel, 0, GrumpyFormatter.Format(message, objects), exception, MessageFormatter);
        }

        private static string MessageFormatter(object state, Exception error)
        {
            return state.ToString();
        }
    }
}
