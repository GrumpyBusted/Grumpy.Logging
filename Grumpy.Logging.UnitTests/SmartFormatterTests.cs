using FluentAssertions;
using Grumpy.Logging.UnitTests.Helper;
using Xunit;

namespace Grumpy.Logging.UnitTests
{
    public class SmartFormatterTests
    {
        [Fact]
        public void SmartFormatFormattingPlainString()
        {
            var s = GrumpyFormatter.Format("Hallo");

            s.Should().Be("Hallo");
        }

        [Fact]
        public void SmartFormatUsingNamedPartOfObject()
        {
            var obj = new { foo = "Value" };

            var s = GrumpyFormatter.Format("Hallo {#foo}", obj);

            s.Should().Be("Hallo {#foo}\r\nHallo \"Value\"\r\n{\r\n  \"foo\": \"Value\"\r\n}");
        }

        [Fact]
        public void SmartFormatSerializeObject()
        {
            var s = GrumpyFormatter.Format("Hallo {@dto}", new MyDto());

            s.Should().Be("Hallo {@dto}\r\nHallo\r\n{\r\n  \"dto\": {\r\n    \"Text\": \"Text\",\r\n    \"Int\": 123,\r\n    \"Number\": 123.45,\r\n    \"Dec\": 987.6,\r\n    \"Date\": \"2017-10-25T00:00:00+02:00\"\r\n  }\r\n}");
        }

        [Fact]
        public void SmartFormatUsingNamedPartOfObjectAndFormattingOption()
        {
            var s = GrumpyFormatter.Format("Hallo {#date:yyyy-MM-dd}", new MyDto());

            s.Should().Be("Hallo {#date:yyyy-MM-dd}\r\nHallo \"2017-10-25\"\r\n{\r\n  \"date\": \"2017-10-25\"\r\n}");
        }

        [Fact]
        public void SmartFormatUsingTwoNamedPartOfObject()
        {
            var obj1 = new { foo = "Value1", bar = "Value2" };
            var obj2 = new { foo = "Value3", bar = "Value4" };

            var s = GrumpyFormatter.Format("Hallo {#foo} {#bar} {#foo}", obj1, obj2);

            s.Should().Be("Hallo {#foo} {#bar} {#foo}\r\nHallo \"Value1\" \"Value4\" \"Value3\"\r\n{\r\n  \"foo\": \"Value1\"\r\n}\r\n{\r\n  \"bar\": \"Value4\"\r\n}\r\n{\r\n  \"foo\": \"Value3\"\r\n}");
        }

        [Fact]
        public void SmartFormatOfInt()
        {
            var s = GrumpyFormatter.Format("Hallo {foo:000}", 12);

            s.Should().Be("Hallo {foo:000}\r\nHallo \"012\"\r\n{\r\n  \"foo\": \"012\"\r\n}");
        }

        [Fact]
        public void SmartFormatUsingNPathIntoObject()
        {
            var obj = new { foo = new { bar = "Value" } };

            var s = GrumpyFormatter.Format("Hallo {#foo.bar}", obj);

            s.Should().Be("Hallo {#foo.bar}\r\nHallo \"Value\"\r\n{\r\n  \"foo.bar\": \"Value\"\r\n}");
        }

        [Fact]
        public void SmartFormatSerializeTwoObjects()
        {
            var obj = new { foo = new { bar = "Value" } };

            var s = GrumpyFormatter.Format("Hallo {@object} {@dto}", obj, new MyDto());

            s.Should().Be("Hallo {@object} {@dto}\r\nHallo\r\n{\r\n  \"object\": {\r\n    \"foo\": {\r\n      \"bar\": \"Value\"\r\n    }\r\n  }\r\n}\r\n{\r\n  \"dto\": {\r\n    \"Text\": \"Text\",\r\n    \"Int\": 123,\r\n    \"Number\": 123.45,\r\n    \"Dec\": 987.6,\r\n    \"Date\": \"2017-10-25T00:00:00+02:00\"\r\n  }\r\n}");
        }

        [Fact]
        public void SmartFormatWithIndexedObjects()
        {
            var s = GrumpyFormatter.Format("Hallo {@1} {0}", 123, new MyDto());

            s.Should().Be("Hallo {@1} {0}\r\nHallo  123\r\n{\r\n  \"Text\": \"Text\",\r\n  \"Int\": 123,\r\n  \"Number\": 123.45,\r\n  \"Dec\": 987.6,\r\n  \"Date\": \"2017-10-25T00:00:00+02:00\"\r\n}");
        }

        [Fact]
        public void SmartFormatWithNulls()
        {
            var s = GrumpyFormatter.Format("Hallo {Var1} {@Var2}", null, null);

            s.Should().Be("Hallo {Var1} {@Var2}\r\nHallo null null\r\nObject not found by formatter: Var1\r\n{\r\n  \"Var1\": null\r\n}\r\nObject not found by formatter: Var2\r\n{\r\n  \"Var2\": null\r\n}");
        }
    }
}
