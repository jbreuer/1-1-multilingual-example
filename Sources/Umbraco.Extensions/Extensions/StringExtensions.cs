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
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the string to MD5
        /// </summary>
        /// <param name="stringToConvert">
        /// referrs to itself
        /// </param>
        /// <returns>
        /// the md5 hashed string
        /// </returns>
        public static string ToMd5(this string stringToConvert)
        {
            // Create an instance of the MD5CryptoServiceProvider.
            var md5Provider = new MD5CryptoServiceProvider();

            // Convert our string into byte array.
            var byteArray = Encoding.UTF8.GetBytes(stringToConvert);

            // Get the hashed values created by our MD5CryptoServiceProvider.
            var hashedByteArray = md5Provider.ComputeHash(byteArray);

            // Create a StringBuilder object.
            var stringBuilder = new StringBuilder();

            // Loop to each each byte.
            foreach (var b in hashedByteArray)
            {
                // Append it to our StringBuilder.
                stringBuilder.Append(b.ToString("x2").ToLower());
            }

            // Return the hashed value.
            return stringBuilder.ToString();
        }
    }
}