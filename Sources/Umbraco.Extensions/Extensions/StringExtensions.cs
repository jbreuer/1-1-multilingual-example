// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The string extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Umbraco.Extensions.Extensions
{
    using System;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// The remove from end.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="suffix">
        /// The suffix.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveAfterLast(this string s, string suffix)
        {
            var index = s.LastIndexOf(suffix, StringComparison.InvariantCulture);
            if (index > 0)
            {
                s = s.Substring(0, index);
            }

            return s;
        }

        /// <summary>
        /// Removed invalid xml characters.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveInvalidXmlChars(this string text)
        {
            if (text == null)
            {
                return null;
            }

            var validXmlChars = text.Where(XmlConvert.IsXmlChar).ToArray();
            return new string(validXmlChars);
        }
    }
}