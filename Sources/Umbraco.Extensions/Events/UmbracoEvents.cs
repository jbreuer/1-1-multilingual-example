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
        protected override void ApplicationStarting(
            UmbracoApplicationBase umbracoApplication, 
            ApplicationContext applicationContext)
        {
            // With the url providers we can change node urls.
            UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, MultilingualUrlProvider>();

            // Remove the DefaultUrlProvider because our MultilingualUrlProvider should take care of all the urls.
            // This only works if there are domains assigned to the home node.
            UrlProviderResolver.Current.RemoveType<DefaultUrlProvider>();
        }
    }
}