namespace Grumpy.Logging
{
    /// <summary>
    /// String Extension for Smart formatter
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Format String with in string serialization e.g. "Hallo {#obj.Name} {@MyObject}".GrumpyFormat(obj, myObj)
        /// </summary>
        /// <param name="template">Template string</param>
        /// <param name="objects">List of objects</param>
        /// <returns>Formatted string</returns>
        public static string GrumpyFormat(this string template, params object[] objects)
        {
            return GrumpyFormatter.Format(template, objects);
        }
    }
}