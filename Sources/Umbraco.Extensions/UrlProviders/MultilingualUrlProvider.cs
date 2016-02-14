// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultilingualUrlProvider.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The multilingual url provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Umbraco.Extensions.UrlProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Umbraco.Core;
    using Umbraco.Extensions.Models;
    using Umbraco.Web;
    using Umbraco.Web.Routing;

    /// <summary>
    /// The multilingual url provider.
    /// </summary>
    public class MultilingualUrlProvider : IUrlProvider
    {
        
        public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
        {
            var content = umbracoContext.ContentCache.GetById(id) as UmbMaster;

            if (content != null && !content.DocumentTypeAlias.InvariantEquals(UmbHomePage.ModelTypeAlias))
            {
                if (!UmbracoContext.Current.IsFrontEndUmbracoRequest)
                {
                    var domains = ApplicationContext.Current.Services.DomainService.GetAll(true).ToList();
                    if (domains.Any())
                    {
                        var urlSegment = content.GetUrlSegment(domains[1].LanguageIsoCode);
                    }
                }
            }

            return null;
        }

        public IEnumerable<string> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
            return null;
        }
    }
}