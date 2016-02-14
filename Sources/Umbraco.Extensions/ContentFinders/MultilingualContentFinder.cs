namespace Umbraco.Extensions.ContentFinders
{
    using System;
    using System.Linq;

    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Extensions.Models;
    using Umbraco.Web;
    using Umbraco.Web.Routing;

    /// <summary>
    /// The multilingual content finder.
    /// </summary>
    public class MultilingualContentFinder : IContentFinder
    {
        /// <summary>
        /// The try find content.
        /// </summary>
        /// <param name="contentRequest">
        /// The content request.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            if (contentRequest == null)
            {
                return false;
            }

            try
            {
                using (
                    ApplicationContext.Current.ProfilingLogger.TraceDuration<MultilingualContentFinder>(
                        "Started TryFindContent", 
                        "Completed TryFindContent"))
                {
                    var helper = new UmbracoHelper(UmbracoContext.Current);

                    // Get the current url without querystring.
                    var url = this.RemoveQueryFromUrl(contentRequest.Uri.ToString()).EnsureEndsWith("/");

                    // Get all the nodes in the website. This could be bad for performance so should be cached somehow.
                    var allNodes = helper.TypedContentAtRoot().DescendantsOrSelf<UmbMaster>();
                    
                    // See if we can find a node that matched the url. When we check for .Url the UrlProvider will return the correct multilingual url. 
                    var content = allNodes.FirstOrDefault(x => x.Url.InvariantEquals(url));

                    if (content != null)
                    {
                        contentRequest.PublishedContent = content;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<MultilingualContentFinder>("Error in contenfinder MultilingualContentFinder", ex);
            }

            return contentRequest.PublishedContent != null;
        }

        /// <summary>
        /// Remove the querystring from a url.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string RemoveQueryFromUrl(string url)
        {
            var index = url.IndexOf('?');

            return index > 0 ? url.Substring(0, index) : url;
        }
    }
}