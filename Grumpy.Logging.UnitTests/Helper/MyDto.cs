using System;
using System.Diagnostics.CodeAnalysis;

namespace Grumpy.Logging.UnitTests.Helper
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal class MyDto
    {
        public string Text { get; set; } = "Text";
        public int Int { get; set; } = 123;
        public double Number { get; set; } = 123.45;
        public decimal Dec { get; set; } = new decimal(987.6);
        public DateTimeOffset Date { get; set; } = DateTime.Parse("2017-10-25");
    }
}