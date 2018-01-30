using System.Collections.Generic;

namespace Grumpy.Logging
{
    internal class LiteralFormat : ITextExpression
    {
        private readonly string _expression;

        public LiteralFormat(string expression)
        {
            _expression = expression;
        }

        public string Evaluate(object[] objects, ref int index, ref ICollection<string> data)
        {
            return _expression.Replace("{{", "{").Replace("}}", "}");
        }
    }
}