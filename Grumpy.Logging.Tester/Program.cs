using System;
using Microsoft.Extensions.Logging.Console;

namespace Grumpy.Logging.Tester
{
    public static class Program
    {
        private static void Main()
        {
            var logger = new ConsoleLogger("Grumpy.Logging.Tester", (s, level) => true, false);

            logger.Warning(new Exception("Error"), "Message");
            logger.Warning("Message {@Dto}", new MyDto());
        }
    }
}
