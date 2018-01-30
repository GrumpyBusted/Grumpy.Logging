using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Grumpy.Logging.UnitTests
{
    public class LoggerExtensionsTests
    {
        private readonly ILogger _logger;

        public LoggerExtensionsTests()
        {
            _logger = Substitute.For<ILogger>();
        }

        [Fact]
        public void CanLogDebug()
        {
            _logger.Debug("Message {0}", 1);
        }

        [Fact]
        public void CanLogDebugWithException()
        {
            _logger.Debug(new Exception(), "Message {0}", 1);
        }

        [Fact]
        public void CanLogTrace()
        {
            _logger.Trace("Message {0}", 1);
        }

        [Fact]
        public void CanLogTraceException()
        {
            _logger.Trace(new Exception(), "Message {0}", 1);
        }

        [Fact]
        public void CanLogInformation()
        {
            _logger.Information("Message {0}", 1);
        }

        [Fact]
        public void CanLogInformationWithException()
        {
            _logger.Information(new Exception(), "Message {0}", 1);
        }

        [Fact]
        public void CanLogWarning()
        {
            _logger.Warning("Message {0}", 1);
        }

        [Fact]
        public void CanLogWarningWithException()
        {
            _logger.Warning(new Exception(), "Message {0}", 1);
        }

        [Fact]
        public void CanLogError()
        {
            _logger.Error("Message {0}", 1);
        }

        [Fact]
        public void CanLogErrorWithException()
        {
            _logger.Error(new Exception(), "Message {0}", 1);
        }

        [Fact]
        public void CanLogCritical()
        {
            _logger.Critical("Message {0}", 1);
        }

        [Fact]
        public void CanLogCriticalWithException()
        {
            _logger.Critical(new Exception(), "Message {0}", 1);
        }
    }
}
