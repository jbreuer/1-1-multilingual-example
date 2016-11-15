namespace Umbraco.Extensions.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Umbraco.Extensions.Models.Custom;
    using Umbraco.Web.Mvc;

    /// <summary>
    /// Getting preview URLs from the backend doesn't work. That's why we get the URLs from the frontend of the website.
    /// </summary>
    public class UrlPreviewController : SurfaceController
    {
        /// <summary>
        /// Get the URLs from the frontend.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public ActionResult GetUrls(int id)
        {
            var urls = new List<string>();

            if (id > 0)
            {
                urls.Add(this.UmbracoContext.UrlProvider.GetUrl(id));
                urls.AddRange(this.UmbracoContext.UrlProvider.GetOtherUrls(id));
            }

            var urlPreview = new UrlPreview
            {
                IsPreview = this.UmbracoContext.InPreviewMode,
                Urls = urls
            };

            return this.Json(urlPreview, JsonRequestBehavior.AllowGet);
        }
    }
}