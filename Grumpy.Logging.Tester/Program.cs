using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Grumpy.Logging.Tester
{
    public static class Program
    {
        private static void Main()
        {
            var logger = new ConsoleLogger("Grumpy.Logging.Tester", (s, level) => true, true);

            logger.BeginScope("Hallo");
            logger.Warning(new Exception("Error"), "Message");
            new MyClass(logger).MyLog("Message");
            using (logger.BeginScope("World"))
            {
                logger.Warning("Message {@Dto}", new MyDto());
            }
            logger.Warning("More");
        }
    }

    public class MyClass
    {
        private readonly ILogger _logger;

        public MyClass(ILogger logger)
        {
            _logger = logger;
            _logger.BeginScope(GetType().Name);
        }

        public void MyLog(string message)
        {
            _logger.Information("More");
        }
    }
}
