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
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Hosting;

    using Examine;

    using Umbraco.Core;
    using Umbraco.Core.Configuration;
    using Umbraco.Core.Events;
    using Umbraco.Core.Models;
    using Umbraco.Core.Publishing;
    using Umbraco.Core.Services;
    using Umbraco.Extensions.ContentFinders;
    using Umbraco.Extensions.Extensions;
    using Umbraco.Extensions.Extract;
    using Umbraco.Extensions.Models.Custom;
    using Umbraco.Extensions.Models.Custom.Extract;
    using Umbraco.Extensions.UrlProviders;
    using Umbraco.Web;
    using Umbraco.Web.Cache;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Models.ContentEditing;
    using Umbraco.Web.Routing;
    using Umbraco.Web.Security;

    using UmbracoExamine;

    /// <summary>
    /// The umbraco events.
    /// </summary>
    public class UmbracoEvents : ApplicationEventHandler
    {
        private UmbracoHelper UmbracoHelper { get; set; }

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
            PageCacheRefresher.CacheUpdated += this.PageCacheRefresherCacheUpdated;
            ContentService.Saved += this.ContentServiceSaved;

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

        /// <summary>
        /// Override for ApplicationStarted
        /// </summary>
        /// <param name="umbracoApplication">
        /// Umbraco application.
        /// </param>
        /// <param name="applicationContext">
        /// Umbraco application context.
        /// </param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var externalIndexer = (UmbracoContentIndexer)ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"];
            externalIndexer.GatheringNodeData += this.ExternalIndexerGatheringContentData;
            EditorModelEventManager.SendingContentModel += this.EditorModelEventManagerSendingContentModel;
        }

        /// <summary>
        /// The application initialized event
        /// </summary>
        /// <param name="umbracoApplication">
        /// The umbraco application.
        /// </param>
        /// <param name="applicationContext">
        /// The application context.
        /// </param>
        protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Resolve all property extract classes.
            PropertyExtractResolver.Current = new PropertyExtractResolver(PluginManager.Current.ResolveTypes<PropertyExtractBase>());
        }

        private void PageCacheRefresherCacheUpdated(PageCacheRefresher sender, Core.Cache.CacheRefresherEventArgs e)
        {
            // After content has been updated clear content finder cache.
            this.ClearCache();
        }

        private void ContentServiceSaved(IContentService sender, SaveEventArgs<IContent> e)
        {
            // The cms should be on a separate server. So in the save event the cache will only be cleared on that server.
            // This will make the preview update to date after saving.
            this.ClearCache();
        }

        private void ExternalIndexerGatheringContentData(object sender, IndexingNodeDataEventArgs indexingNodeDataEventArgs)
        {
            this.EnsureUmbracoContext();

            var content = this.UmbracoHelper.TypedContent(indexingNodeDataEventArgs.NodeId);
            if (content != null)
            {
                foreach (var lang in ApplicationContext.Current.Services.LocalizationService.GetAllLanguages())
                {
                    var searhContentBuilder = new StringBuilder();
                    content.ExtractForExamine(searhContentBuilder, lang.CultureInfo.Name);

                    // Index all properties per language. For Vorto this means the properties for that language.
                    // If a property is not multilingual it will be added to all languages.
                    indexingNodeDataEventArgs.Fields.Add("PageContent" + "-" + lang.CultureInfo.Name, searhContentBuilder.ToString());
                }
            }
        }
        
        private void EditorModelEventManagerSendingContentModel(System.Web.Http.Filters.HttpActionExecutedContext sender, EditorModelEventArgs<ContentItemDisplay> e)
        {
            var contentModel = e.Model;
            if (contentModel != null)
            {
                contentModel.AllowPreview = false;
            }
        }

        private void EnsureUmbracoContext()
        {
            if (UmbracoContext.Current == null)
            {
                var dummyHttpContext = new HttpContextWrapper(new HttpContext(new SimpleWorkerRequest(string.Empty, string.Empty, new StringWriter())));

                this.UmbracoHelper =
                    new UmbracoHelper(
                        UmbracoContext.EnsureContext(
                            dummyHttpContext,
                            ApplicationContext.Current,
                            new WebSecurity(dummyHttpContext, ApplicationContext.Current),
                            UmbracoConfig.For.UmbracoSettings(),
                            UrlProviderResolver.Current.Providers,
                            false));
            }
        }

        private void ClearCache()
        {
            // clear our runtime cache
            ApplicationContext.Current.ApplicationCache.RuntimeCache.ClearCacheByKeySearch("MultilingualContentFinder");
        }
    }
}