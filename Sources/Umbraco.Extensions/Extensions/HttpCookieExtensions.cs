// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentExtensions.cs" company="Colours B.V.">
//   © Colours B.V. 2016
// </copyright>
// <summary>
//   The conten extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Umbraco.Extensions.Extensions
{
    using System.Linq;
    using System.Net.Http;
    using System.Web;

    /// <summary>
    /// The string extensions.
    /// </summary>
    public static class HttpCookieExtensions
    {
        /// <summary>
        /// The has preview cookie.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool HasPreviewCookie(this HttpRequest request)
        {
            return request.Cookies["UMB_PREVIEW"] != null;
        }

        /// <summary>
        /// The get preview cookie value.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetPreviewCookieValue(this HttpRequestMessage request)
        {
            var cookie = request.Headers.GetCookies("UMB_PREVIEW").FirstOrDefault();
            if (cookie != null)
            {
                if (cookie["UMB_PREVIEW"] != null)
                {
                    return cookie["UMB_PREVIEW"].Value;
                }
            }

            return null;
        }
    }
}