using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Grumpy.Logging.UnitTests
{
    public class LoggerExtensionsTests
    {
        private readonly ILogger _logger;

        public LoggerExtensionsTests()
        {
            _logger = NullLogger.Instance;
        }

        [Fact]
        public void CanLogWarningWithException()
        {
            _logger.LogWarning(new Exception(), "Message");
        }
    }
}
