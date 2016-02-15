namespace Umbraco.Extensions.UrlProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Umbraco.Core;
    using Umbraco.Extensions.Models;
    using Umbraco.Web;
    using Umbraco.Web.Routing;

    /// <summary>
    /// The multilingual url provider.
    /// </summary>
    public class MultilingualUrlProvider : IUrlProvider
    {
        /// <summary>
        /// The get url.
        /// </summary>
        /// <param name="umbracoContext">
        /// The umbraco context.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="current">
        /// The current.
        /// </param>
        /// <param name="mode">
        /// The mode.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
        {
            var content = umbracoContext.ContentCache.GetById(id) as UmbMaster;

            if (content != null)
            {
                var domains = ApplicationContext.Current.Services.DomainService.GetAll(true).OrderBy(x => x.Id).ToList();
                if (domains.Any())
                {
                    // Don't use umbracoContext.PublishedContentRequest.Culture because this code is also called in the backend.
                    var currentCulture = Thread.CurrentThread.CurrentCulture.ToString();

                    // On the frontend get the domain that matches the current culture. We could also check the domain name, but for now the culture is enough.
                    // Otherwise just get the first domain. The urls for the other domains are generated in the GetOtherUrls method.
                    var domain = UmbracoContext.Current.IsFrontEndUmbracoRequest ? domains.First(x => x.LanguageIsoCode.InvariantEquals(currentCulture)) : domains.First();

                    if (content.DocumentTypeAlias.InvariantEquals(UmbHomePage.ModelTypeAlias))
                    {
                        // Return the domain if we're on the homepage because on that node we've added the domains.
                        return domain.DomainName.EnsureEndsWith("/");
                    }

                    // Get the parent url and add the url segment of this culture.
                    var parentUrl = umbracoContext.UrlProvider.GetUrl(content.Parent.Id);
                    var urlSegment = content.GetUrlSegment(domain.LanguageIsoCode);
                    return parentUrl.EnsureEndsWith("/") + urlSegment;
                }
            }

            return null;
        }

        /// <summary>
        /// The get other urls.
        /// </summary>
        /// <param name="umbracoContext">
        /// The umbraco context.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="current">
        /// The current.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<string> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
            var content = umbracoContext.ContentCache.GetById(id) as UmbMaster;

            if (content != null)
            {
                if (!UmbracoContext.Current.IsFrontEndUmbracoRequest)
                {
                    var domains = ApplicationContext.Current.Services.DomainService.GetAll(true).OrderBy(x => x.Id).ToList();
                    if (domains.Count > 1)
                    {
                        var urls = new List<string>();

                        // Skip the first domain because it's already used in the GetUrl method.
                        // We don't need to check IsFrontEndUmbracoRequest here because this code is only called in the backend.
                        domains = domains.Skip(1).ToList();

                        if (content.DocumentTypeAlias.InvariantEquals(UmbHomePage.ModelTypeAlias))
                        {
                            // Return the domain if we're on the homepage because on that node we've added the domains.
                            urls.AddRange(domains.Select(x => x.DomainName.EnsureEndsWith("/")));
                        }
                        else
                        {
                            // Get the other urls for the parent which aren't the main url.
                            var parentUrls = umbracoContext.UrlProvider.GetOtherUrls(content.Parent.Id).ToList();

                            for (int i = 0; i < domains.Count; i++)
                            {
                                // Get the domain and matching parent url.
                                var domain = domains[i];
                                var parentUrl = parentUrls[i];

                                // Get the parent url and add the url segment of this culture.
                                var urlSegment = content.GetUrlSegment(domain.LanguageIsoCode);
                                urls.Add(parentUrl.EnsureEndsWith("/") + urlSegment);
                            }
                        }

                        return urls;
                    }
                }
            }

            return null;
        }
    }
}