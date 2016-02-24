namespace Umbraco.Extensions.ContentFinders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Umbraco.Core;
    using Umbraco.Core.Cache;
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

                    var urls = UmbracoContext.Current.Application.ApplicationCache.RuntimeCache.GetCacheItem<List<Tuple<int, string>>>(
                        "MultilingualContentFinder-Urls",
                        () =>
                        {
                            // Get all the nodes in the website.
                            var allNodes = helper.TypedContentAtRoot().DescendantsOrSelf<UmbMaster>().ToList();

                            // Get all the urls in the website.
                            return allNodes.Select(x => new Tuple<int, string>(x.Id, x.Url)).ToList();
                        });

                    if (urls.Any())
                    {
                        // Get the current url without querystring.
                        var url = this.RemoveQueryFromUrl(contentRequest.Uri.ToString()).EnsureEndsWith("/");

                        var currentUrlItem = urls.FirstOrDefault(x => url.Equals(x.Item2));

                        if (currentUrlItem != null)
                        {
                            var contentItem = UmbracoContext.Current.ContentCache.GetById(currentUrlItem.Item1);

                            if (contentItem != null)
                            {
                                contentRequest.PublishedContent = contentItem;
                            }
                        }
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