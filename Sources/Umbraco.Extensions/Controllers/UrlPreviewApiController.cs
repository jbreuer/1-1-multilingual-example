namespace Umbraco.Extensions.Controllers
{
    using System.Collections.Generic;

    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    /// <summary>
    /// The url preview api controller.
    /// </summary>
    [PluginController("UrlPreview")]
    public class UrlPreviewApiController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// The get urls.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<string> GetUrls(int id)
        {
            var urls = new List<string>();

            if (id > 0)
            {
                urls.Add(this.UmbracoContext.UrlProvider.GetUrl(id));
                urls.AddRange(this.UmbracoContext.UrlProvider.GetOtherUrls(id));
            }

            return urls;
        }
    }
}