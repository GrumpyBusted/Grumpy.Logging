using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Grumpy.Common.Extensions;
using Grumpy.Json;

namespace Grumpy.Logging
{
    internal class FormatExpression : ITextExpression
    {
        private readonly bool _invalidExpression;
        private readonly char _type;
        private readonly string _expression;
        private readonly string _format;
        private readonly int _index = -1;

        public FormatExpression(string expression)
        {
            if (!expression.StartsWith("{") || !expression.EndsWith("}"))
            {
                _invalidExpression = true;
                _expression = expression;
                return;
            }

            var expressionWithoutBraces = expression.Substring(1, expression.Length - 2);

            var colonIndex = expressionWithoutBraces.IndexOf(':');

            if (colonIndex < 0)
                _expression = expressionWithoutBraces.Trim();
            else
            {
                _expression = expressionWithoutBraces.Substring(0, colonIndex).Trim();
                _format = expressionWithoutBraces.Substring(colonIndex + 1).Trim();
            }

            _type = _expression.Length > 0 ? _expression[0] : ' ';

            if (_type == '@' || _type == '#')
                _expression = _expression.Substring(1).Trim();
            else
                _type = ' ';

            if (!string.IsNullOrEmpty(_format) && _type == '@')
            {
                _invalidExpression = true;
                _expression = expression;
            }

            if (_type != '#' && int.TryParse(_expression, out var expressionIndex))
            {
                _expression = "";
                _index = expressionIndex;
            }
        }

        /// <inheritdoc />
        public string Evaluate(object[] objects, ref int index, ref ICollection<string> data)
        {
            var obj = GetCurrentObject(objects, ref index, data);

            if (_invalidExpression)
                data.Add($"Invalid expression: {_expression} {obj}");

            if (obj == null)
                data.Add($"Object not found by formatter: {_expression}");

            try
            {
                string value = null;

                switch (_type)
                {
                    case '@':
                        value = obj.SerializeToJson();
                        break;
                    case '#' when string.IsNullOrEmpty(_format):
                        obj = DataBinder.Eval(obj, _expression);
                        break;
                    case '#':
                        obj = DataBinder.Eval(obj, _expression, "{0:" + _format + "}");
                        break;
                    default:
                        // ReSharper disable FormatStringProblem
                        if (!string.IsNullOrEmpty(_format))
                            value = string.Format("\"{0:" + _format + "}\"", obj);
                        // ReSharper restore FormatStringProblem
                        break;
                }

                return FormatOutput(data, obj, value);
            }
            catch (ArgumentException)
            {
                data.Add($"Invalid argument for formatting: {_expression} {obj}");
            }
            catch (HttpException)
            {
                data.Add($"Unable to format expression: {_expression} {obj}");
            }
            catch (Exception)
            {
                data.Add($"Exception formatting expression: {_expression} {obj}");
            }

            return "";
        }

        private string FormatOutput(ICollection<string> data, object obj, string value)
        {
            if (obj == null)
                value = "null";
            else if (value == null)
            {
                if (obj.IsNumber())
                    value = obj.ToString();
                else
                    value = "\"" + obj + "\"";
            }

            if (_expression != "")
                data.Add("{\r\n  \"" + _expression + "\": " + value.Replace("\r\n", "\r\n  ") + "\r\n}");
            else if (value.Left() == "{")
                data.Add(value);

            if (value.Left() == "{")
                value = "";

            return value;
        }

        private object GetCurrentObject(IReadOnlyList<object> objects, ref int index, ICollection<string> data)
        {
            object obj = null;

            if (_index >= 0)
            {
                if (_index.Between(0, objects.Count - 1))
                    obj = objects[_index];
                else
                    data.Add($"Expression index of out bound {_expression}");
            }
            else
                obj = objects[Math.Min(index, objects.Count - 1)];

            ++index;
            return obj;
        }
    }
}