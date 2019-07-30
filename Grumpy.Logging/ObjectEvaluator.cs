using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Grumpy.Logging
{
    /// <summary>
    /// Copied from System.Web.UI.DataBinder (Framework) to be used in .NET Standard library
    /// </summary>
    internal static class ObjectEvaluator
    {
        private static readonly char[] ExpressionPartSeparator = {'.'};
        private static readonly char[] IndexExprStartChars = {'[', '('};
        private static readonly char[] IndexExprEndChars = {']', ')'};
        private static readonly ConcurrentDictionary<Type, PropertyDescriptorCollection> PropertyCache = new ConcurrentDictionary<Type, PropertyDescriptorCollection>();

        public static object Eval(object container, string expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            expression = expression.Trim();

            if (expression.Length == 0)
                throw new ArgumentNullException(nameof(expression));

            if (container == null)
                return null;

            var expressionParts = expression.Split(ExpressionPartSeparator);

            return Eval(container, expressionParts);
        }

        public static string Eval(object container, string expression, string format)
        {
            var obj = Eval(container, expression);

            if (obj == null || obj == DBNull.Value)
                return string.Empty;

            return string.IsNullOrEmpty(format) ? obj.ToString() : string.Format(format, obj);
        }

        private static object Eval(object container, IReadOnlyList<string> expressionParts)
        {
            var container1 = container;

            for (var index = 0; index < expressionParts.Count && container1 != null; ++index)
            {
                var expressionPart = expressionParts[index];
                container1 = expressionPart.IndexOfAny(IndexExprStartChars) >= 0 ? GetIndexedPropertyValue(container1, expressionPart) : GetPropertyValue(container1, expressionPart);
            }

            return container1;
        }

        public static string GetPropertyValue(object container, string propName, string format)
        {
            var propertyValue = GetPropertyValue(container, propName);

            if (propertyValue == null || propertyValue == DBNull.Value)
                return string.Empty;

            return string.IsNullOrEmpty(format) ? propertyValue.ToString() : string.Format(format, propertyValue);
        }

        private static PropertyDescriptorCollection GetPropertiesFromCache(object container)
        {
            if (container is ICustomTypeDescriptor)
                return TypeDescriptor.GetProperties(container);

            var type = container.GetType();

            if (!PropertyCache.TryGetValue(type, out var descriptorCollection))
            {
                descriptorCollection = TypeDescriptor.GetProperties(type);
                PropertyCache.TryAdd(type, descriptorCollection);
            }

            return descriptorCollection;
        }

        private static object GetPropertyValue(object container, string propName)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            if (string.IsNullOrEmpty(propName))
                throw new ArgumentNullException(nameof(propName));

            var propertyDescriptor = GetPropertiesFromCache(container).Find(propName, true);

            if (propertyDescriptor != null)
                return propertyDescriptor.GetValue(container);

            throw new ArgumentException($"Unable to find property {propName} in {container.GetType().FullName}");
        }

#pragma warning disable S3776 // Copied from the internet
        private static object GetIndexedPropertyValue(object container, string expr)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            if (string.IsNullOrEmpty(expr))
                throw new ArgumentNullException(nameof(expr));

            var obj1 = (object) null;
            var flag = false;
            var length = expr.IndexOfAny(IndexExprStartChars);
            var num = expr.IndexOfAny(IndexExprEndChars, length + 1);

            if (length < 0 || num < 0 || num == length + 1)
                throw new ArgumentException($"Invalid indexed expression - {expr}");

            var propName = (string) null;
            var obj2 = (object) null;
            var s = expr.Substring(length + 1, num - length - 1).Trim();

            if (length != 0)
                propName = expr.Substring(0, length);

            if (s.Length != 0)
            {
                if (s[0] == '"' && s[s.Length - 1] == '"' || s[0] == '\'' && s[s.Length - 1] == '\'')
                    obj2 = s.Substring(1, s.Length - 2);
                else if (char.IsDigit(s[0]))
                {
                    flag = int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result);
                    obj2 = !flag ? s : (object) result;
                }
                else
                    obj2 = s;
            }

            if (obj2 == null)
                throw new ArgumentException($"Invalid indexed expression - {expr}");

            var obj3 = string.IsNullOrEmpty(propName) ? container : GetPropertyValue(container, propName);

            if (obj3 != null)
            {
                switch (obj3)
                {
                    case Array array when flag:
                        obj1 = array.GetValue((int) obj2);
                        break;
                    case IList list when flag:
                        obj1 = list[(int) obj2];
                        break;
                    default:
                    {
                        var property = obj3.GetType().GetProperty("Item", BindingFlags.Instance | BindingFlags.Public, null, null, new[] {obj2.GetType()}, null);

                        if (property != null)
                            obj1 = property.GetValue(obj3, new[] {obj2});
                        else
                            throw new ArgumentException($"Missing indexed accessor - {obj3.GetType().FullName}");
                        break;
                    }
                }
            }

            return obj1;
        }
#pragma warning restore
    }
}