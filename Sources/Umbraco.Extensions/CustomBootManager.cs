namespace Umbraco.Extensions
{
    using System.Web;

    using Umbraco.Core;
    using Umbraco.Core.Cache;
    using Umbraco.Web;

    /// <summary>
    /// The custom boot manager.
    /// </summary>
    public class CustomBootManager : WebBootManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomBootManager"/> class.
        /// </summary>
        /// <param name="umbracoApplication">
        /// The umbraco application.
        /// </param>
        public CustomBootManager(UmbracoApplicationBase umbracoApplication)
            : base(umbracoApplication)
        {
        }

        /// <summary>
        /// The create application cache.
        /// </summary>
        /// <returns>
        /// The <see cref="CacheHelper"/>.
        /// </returns>
        protected override CacheHelper CreateApplicationCache()
        {
            var cacheHelper =
                new CacheHelper(
                    new DeepCloneRuntimeCacheProvider(new HttpRuntimeCacheProvider(HttpRuntime.Cache)), 
                    new StaticCacheProvider(), 
                    new HttpRequestCacheProvider(), 
                    new IsolatedRuntimeCache(
                        type => new DeepCloneRuntimeCacheProvider(new ObjectCacheRuntimeCacheProvider())));

            return cacheHelper;
        }
    }
}