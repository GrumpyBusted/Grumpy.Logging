using System.Collections.Generic;
using System.Linq;

namespace Grumpy.Logging
{
    /// <summary>
    /// Formatter class for in string serialization and other extensions to string format
    /// </summary>
    public static class GrumpyFormatter
    {
        /// <summary>
        /// Format String with in string serialization e.g. GrumpyFormatter.Format("Hallo {#obj.Name} {@MyObject}", obj, myObj)
        /// </summary>
        /// <param name="template">String Template</param>
        /// <param name="objects">List of objects</param>
        /// <returns>Formatted String</returns>
        public static string Format(string template, params object[] objects)
        {
            var objectIndex = 0;
            ICollection<string> data = new List<string>();

            if (template == null)
                return "";

            var res = template;

            if (objects.Length > 0)
            {
                res += "\r\n";
                res += string.Join("", from expression in SplitFormat(template) select expression.Evaluate(objects, ref objectIndex, ref data)).Trim();
            }

            return data.Aggregate(res, (current, s) => current + "\r\n" + s);
        }

        private static IEnumerable<ITextExpression> SplitFormat(string template)
        {
            var exprEndIndex = -1;
            int expStartIndex;

            do
            {
                expStartIndex = template.IndexOfExpressionStart(exprEndIndex + 1);

                if (expStartIndex < 0)
                {
                    if (exprEndIndex + 1 < template.Length)
                        yield return new LiteralFormat(template.Substring(exprEndIndex + 1));

                    break;
                }

                if (expStartIndex - exprEndIndex - 1 > 0)
                    yield return new LiteralFormat(template.Substring(exprEndIndex + 1, expStartIndex - exprEndIndex - 1));

                var endBraceIndex = template.IndexOfExpressionEnd(expStartIndex + 1);

                if (endBraceIndex < 0)
                    yield return new FormatExpression(template.Substring(expStartIndex));
                else
                {
                    exprEndIndex = endBraceIndex;
                    yield return new FormatExpression(template.Substring(expStartIndex, endBraceIndex - expStartIndex + 1));
                }
            } while (expStartIndex > -1);
        }

        private static int IndexOfExpressionStart(this string format, int startIndex)
        {
            while (true)
            {
                var index = format.IndexOf('{', startIndex);

                if (index == -1)
                    return index;

                if (index + 1 < format.Length)
                {
                    var nextChar = format[index + 1];

                    if (nextChar == '{')
                        startIndex = index + 2;
                    else
                        return index;
                }
                else
                    return index;
            }
        }

        private static int IndexOfExpressionEnd(this string format, int startIndex)
        {
            while (true)
            {
                var endBraceIndex = format.IndexOf('}', startIndex);

                if (endBraceIndex == -1)
                    return endBraceIndex;

                var braceCount = 0;

                for (var i = endBraceIndex + 1; i < format.Length; i++)
                {
                    if (format[i] == '}')
                        braceCount++;
                    else
                        break;
                }

                if (braceCount % 2 == 1)
                    startIndex = endBraceIndex + braceCount + 1;
                else
                    return endBraceIndex;
            }
        }
    }
}