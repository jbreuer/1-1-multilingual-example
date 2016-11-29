namespace Umbraco.Extensions.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Umbraco.Core;
    using Umbraco.Extensions.Extensions;
    using Umbraco.Extensions.Models.Custom.Extract;
    using Umbraco.Web;
    using Umbraco.Web.WebApi;

    /// <summary>
    /// The extract api controller.
    /// </summary>
    public class ExtractApiController : UmbracoAuthorizedApiController
    {
        /// <summary>
        /// The get languages.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        [System.Web.Http.HttpGet]
        public IEnumerable<KeyValuePair<string, string>> GetLanguages()
        {
            var languages = this.UmbracoContext.Application.Services.LocalizationService.GetAllLanguages();
            return languages.Select(
                l => new KeyValuePair<string, string>(l.CultureInfo.Name, l.CultureInfo.DisplayName));
        }

        /// <summary>
        /// Extract content so it can be exported.
        /// </summary>
        /// <param name="nodeid">
        /// The nodeid.
        /// </param>
        /// <param name="sourceLanguage">
        /// The source language.
        /// </param>
        /// <param name="targetLanguage">
        /// The target language.
        /// </param>
        /// <param name="includeSubNodes">
        /// The include Sub Nodes.
        /// </param>
        /// <returns>
        /// The <see cref="JsonResult"/>.
        /// </returns>
        [System.Web.Http.HttpPost]
        public JsonResult ExtractContent(int nodeid, string sourceLanguage, string targetLanguage, bool includeSubNodes)
        {
            if (nodeid > -1)
            {
                // Get the unpublished version of the current node.
                var content = this.Services.ContentService.GetById(nodeid);

                // Convert it to an IPublishedContent. So this IPublishedContent has the unpublished version.
                var publishedContent = content.ToPublishedContent();

                var collection = new DocumentCollection
                                     {
                                         Id = nodeid, 
                                         Name = publishedContent.Name,
                                         SourceLanguage = sourceLanguage,
                                         TargetLanguage = targetLanguage
                                     };


                // Export the Umbraco properties to a document.
                if (!includeSubNodes)
                {
                    var document = publishedContent.ExtractForExport(sourceLanguage);

                    collection.Documents.Add(document);
                }
                else
                {
                    // Things like DescendantsOrSelf() just work even though this IPublishedContent could be unpublished.
                    var publishedContentWithDescendants = publishedContent.DescendantsOrSelf().ToList();

                    foreach (var publishedContentDescendant in publishedContentWithDescendants)
                    {
                        var document = publishedContentDescendant.ExtractForExport(sourceLanguage);

                        collection.Documents.Add(document);
                    }
                }

                // Serialize the document to xml. This could be used so the content can be exported.
                var xml = collection.SerializeObject();

                return new JsonResult { Data = new { Error = string.Empty, Xml = xml } };
            }

            return new JsonResult { Data = new { Error = "No node id" } };
        }
    }
}