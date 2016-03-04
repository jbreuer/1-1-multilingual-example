namespace Umbraco.Extensions.Models
{
    using Our.Umbraco.Vorto.Extensions;

    using Umbraco.Core.Models;
    using Umbraco.ModelsBuilder;

    /// <summary>
    /// The umb news item.
    /// </summary>
    public partial class UmbNewsItem
    {
        /// <summary>
        /// Gets the news.
        /// </summary>
        [ImplementPropertyType("news")]
        public Newsdetached News
        {
            get
            {
                var news = this.GetVortoValue<IPublishedContent>("news");

                return news == null ? null : new Newsdetached(news);
            }
        }
    }
}