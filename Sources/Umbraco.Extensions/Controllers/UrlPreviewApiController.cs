namespace Umbraco.Extensions.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    using umbraco.cms.businesslogic.web;
    using umbraco.presentation.preview;

    using Umbraco.Core;
    using Umbraco.Extensions.Extensions;
    using Umbraco.Extensions.Models;
    using Umbraco.Web.WebApi;

    /// <summary>
    /// The url preview api controller.
    /// </summary>
    public class UrlPreviewApiController : UmbracoAuthorizedApiController
    {
        /// <summary>
        /// Update preview for the entire website.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage UpdatePreview()
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            // Get the highest node in the tree.
            // If we use this id for preview it will preview the entire website.
            var websiteId = this.GetWebsiteId();

            // Update the preview so we can see the saved changes.
            this.UpdatePreviewContext(websiteId);

            return result;
        }

        /// <summary>
        /// Checks if the user is currently in preview mode and if so will update the preview content for this item.
        /// If it's not in preview a new one will be created.
        /// </summary>
        /// <param name="contentId">
        /// The content id.
        /// </param>
        /// <remarks>
        /// This code uses some obsolete methods, but parts are copied from the Umbraco source and we keep it the same.
        /// </remarks>
        private void UpdatePreviewContext(int contentId)
        {
            // Get the preview id from the cookie.
            var previewId = this.Request.GetPreviewCookieValue();

            // If we have a GUID from the cookie use that.
            Guid id;
            var existingPreview = Guid.TryParse(previewId, out id);

            if (!existingPreview)
            {
                // If we don't have GUID from the cookie yet create a new one.
                id = Guid.NewGuid();
            }

            // Create the preview.
            var d = new Document(contentId);
            var pc = new PreviewContent(this.UmbracoUser, id, false);
            pc.PrepareDocument(this.UmbracoUser, d, true);
            pc.SavePreviewSet();

            if (!existingPreview)
            {
                // Create a new preview cookie if we don't have one.
                pc.ActivatePreviewCookie();
            }
        }

        /// <summary>
        /// Get the id of the website node.
        /// </summary>
        /// <returns>
        /// The id.
        /// </returns>
        private int GetWebsiteId()
        {
            var websiteContentType = this.Services.ContentTypeService.GetContentType(UmbHomePage.ModelTypeAlias);
            var website = this.Services.ContentService.GetContentOfContentType(websiteContentType.Id).FirstOrDefault();

            if (website != null)
            {
                return website.Id;
            }

            return -1;
        }
    }
}