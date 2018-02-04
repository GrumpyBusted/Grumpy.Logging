using FluentAssertions;
using Grumpy.Logging.UnitTests.Helper;
using Xunit;

namespace Grumpy.Logging.UnitTests
{
    public class GrumpyFormatterTests
    {
        [Fact]
        public void GrumpyFormatFormattingPlainString()
        {
            var s = GrumpyFormatter.Format("Hallo");

            s.Should().Be("Hallo");
        }

        [Fact]
        public void GrumpyFormatUsingNamedPartOfObject()
        {
            var obj = new { foo = "Value" };

            var s = GrumpyFormatter.Format("Hallo {#foo}", obj);

            s.Should().Be("Hallo {#foo}\r\nHallo \"Value\"\r\n{\r\n  \"foo\": \"Value\"\r\n}");
        }

        [Fact]
        public void GrumpyFormatSerializeObject()
        {
            var s = GrumpyFormatter.Format("Hallo {@dto}", new MyDto());

            s.Should().Be("Hallo {@dto}\r\nHallo\r\n{\r\n  \"dto\": {\r\n    \"Text\": \"Text\",\r\n    \"Int\": 123,\r\n    \"Number\": 123.45,\r\n    \"Dec\": 987.6,\r\n    \"Date\": \"2017-10-25T00:00:00+02:00\"\r\n  }\r\n}");
        }

        [Fact]
        public void GrumpyFormatSkipObject()
        {
            var s = GrumpyFormatter.Format("Hallo {%dto}", "Message");

            s.Should().Be("Hallo {%dto}\r\nHallo\r\n{\r\n  \"dto\": \"Message\"\r\n}");
        }

        [Fact]
        public void GrumpyFormatUsingNamedPartOfObjectAndFormattingOption()
        {
            var s = GrumpyFormatter.Format("Hallo {#date:yyyy-MM-dd}", new MyDto());

            s.Should().Be("Hallo {#date:yyyy-MM-dd}\r\nHallo \"2017-10-25\"\r\n{\r\n  \"date\": \"2017-10-25\"\r\n}");
        }

        [Fact]
        public void GrumpyFormatUsingTwoNamedPartOfObject()
        {
            var obj1 = new { foo = "Value1", bar = "Value2" };
            var obj2 = new { foo = "Value3", bar = "Value4" };

            var s = GrumpyFormatter.Format("Hallo {#foo} {#bar} {#foo}", obj1, obj2);

            s.Should().Be("Hallo {#foo} {#bar} {#foo}\r\nHallo \"Value1\" \"Value4\" \"Value3\"\r\n{\r\n  \"foo\": \"Value1\"\r\n}\r\n{\r\n  \"bar\": \"Value4\"\r\n}\r\n{\r\n  \"foo\": \"Value3\"\r\n}");
        }

        [Fact]
        public void GrumpyFormatOfInt()
        {
            var s = GrumpyFormatter.Format("Hallo {foo:000}", 12);

            s.Should().Be("Hallo {foo:000}\r\nHallo \"012\"\r\n{\r\n  \"foo\": \"012\"\r\n}");
        }

        [Fact]
        public void GrumpyFormatUsingNPathIntoObject()
        {
            var obj = new { foo = new { bar = "Value" } };

            var s = GrumpyFormatter.Format("Hallo {#foo.bar}", obj);

            s.Should().Be("Hallo {#foo.bar}\r\nHallo \"Value\"\r\n{\r\n  \"foo.bar\": \"Value\"\r\n}");
        }

        [Fact]
        public void GrumpyFormatSerializeTwoObjects()
        {
            var obj = new { foo = new { bar = "Value" } };

            var s = GrumpyFormatter.Format("Hallo {@object} {@dto}", obj, new MyDto());

            s.Should().Be("Hallo {@object} {@dto}\r\nHallo\r\n{\r\n  \"object\": {\r\n    \"foo\": {\r\n      \"bar\": \"Value\"\r\n    }\r\n  }\r\n}\r\n{\r\n  \"dto\": {\r\n    \"Text\": \"Text\",\r\n    \"Int\": 123,\r\n    \"Number\": 123.45,\r\n    \"Dec\": 987.6,\r\n    \"Date\": \"2017-10-25T00:00:00+02:00\"\r\n  }\r\n}");
        }

        [Fact]
        public void GrumpyFormatWithIndexedObjects()
        {
            var s = GrumpyFormatter.Format("Hallo {@1} {0}", 123, new MyDto());

            s.Should().Be("Hallo {@1} {0}\r\nHallo  123\r\n{\r\n  \"Text\": \"Text\",\r\n  \"Int\": 123,\r\n  \"Number\": 123.45,\r\n  \"Dec\": 987.6,\r\n  \"Date\": \"2017-10-25T00:00:00+02:00\"\r\n}");
        }

        [Fact]
        public void GrumpyFormatWithNulls()
        {
            var s = GrumpyFormatter.Format("Hallo {Var1} {@Var2}", null, null);

            s.Should().Be("Hallo {Var1} {@Var2}\r\nHallo null\r\nObject not found by formatter: Var1\r\n{\r\n  \"Var1\": null\r\n}\r\nObject not found by formatter: Var2\r\n{\r\n  \"Var2\": null\r\n}");
        }
    }
}
