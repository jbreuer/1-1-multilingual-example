// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UmbracoEvents.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The umbraco events.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Umbraco.Extensions.Events
{
    using Umbraco.Core;
    using Umbraco.Core.Events;
    using Umbraco.Core.Models;
    using Umbraco.Core.Publishing;
    using Umbraco.Core.Services;
    using Umbraco.Extensions.ContentFinders;
    using Umbraco.Extensions.UrlProviders;
    using Umbraco.Web.Routing;

    /// <summary>
    /// The umbraco events.
    /// </summary>
    public class UmbracoEvents : ApplicationEventHandler
    {
        /// <summary>
        /// The application starting.
        /// </summary>
        /// <param name="umbracoApplication">
        /// The umbraco application.
        /// </param>
        /// <param name="applicationContext">
        /// The application context.
        /// </param>
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Events.
            ContentService.Published += this.ContentPublished;
            ContentService.UnPublished += this.ContentUnpublished;
            ContentService.Moved += this.ContentMoved;
            ContentService.Trashed += this.ContentTrashed;
            ContentService.Deleted += this.ContentDeleted;

            // With the url providers we can change node urls.
            UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, MultilingualUrlProvider>();

            // Remove the DefaultUrlProvider because our MultilingualUrlProvider should take care of all the urls.
            // This only works if there are domains assigned to the home node.
            UrlProviderResolver.Current.RemoveType<DefaultUrlProvider>();

            // With the content finder we can match nodes to urls.
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNiceUrl, MultilingualContentFinder>();

            // Remove the ContentFinderByNiceUrl because our MultilingualContentFinder should find all the content.
            ContentFinderResolver.Current.RemoveType<ContentFinderByNiceUrl>();
        }

        private void ContentPublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            this.ClearCache();
        }

        private void ContentDeleted(IContentService sender, DeleteEventArgs<IContent> e)
        {
            this.ClearCache();
        }

        private void ContentTrashed(IContentService sender, MoveEventArgs<IContent> e)
        {
            this.ClearCache();
        }

        private void ContentMoved(IContentService sender, MoveEventArgs<IContent> e)
        {
            this.ClearCache();
        }

        private void ContentUnpublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            this.ClearCache();
        }

        private void ClearCache()
        {
            // After content has been updated clear content finder cache.
            ApplicationContext.Current.ApplicationCache.RuntimeCache.ClearCacheByKeySearch("MultilingualContentFinder");
        }
    }
}