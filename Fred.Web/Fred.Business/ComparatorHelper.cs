using System.Globalization;

namespace Fred.Business
{
    /// <summary>
    /// Comparator helper
    /// </summary>
    public static class ComparatorHelper
    {
        /// <summary>
        /// compare two strings
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="value">Value</param>
        /// <returns>boolean</returns>
        public static bool ComplexContains(string source, string value)
        {
            var index = CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                source, value, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
            return index != -1;
        }
    }
}
