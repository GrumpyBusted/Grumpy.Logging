using FluentAssertions;
using Grumpy.Logging.UnitTests.Helper;
using Xunit;

namespace Grumpy.Logging.UnitTests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void GrumpyFormatExtensionSerializeObject()
        {
            var obj = new { foo = new { bar = "Value" } };

            var s = "Hi {@object} {@dto}".GrumpyFormat(obj, new MyDto());

            s.Should().Be("Hi {@object} {@dto}\r\nHi\r\n{\r\n  \"object\": {\r\n    \"foo\": {\r\n      \"bar\": \"Value\"\r\n    }\r\n  }\r\n}\r\n{\r\n  \"dto\": {\r\n    \"Text\": \"Text\",\r\n    \"Int\": 123,\r\n    \"Number\": 123.45,\r\n    \"Dec\": 987.6,\r\n    \"Date\": \"2017-10-25T00:00:00+02:00\"\r\n  }\r\n}");
        }
    }
}