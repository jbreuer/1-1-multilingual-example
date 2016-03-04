namespace Umbraco.Extensions.Models
{
    using Our.Umbraco.Vorto.Extensions;

    using Umbraco.Core;
    using Umbraco.ModelsBuilder;

    /// <summary>
    /// The umb master.
    /// </summary>
    public partial class UmbMaster
    {
        /// <summary>
        /// Gets the url segment.
        /// </summary>
        [ImplementPropertyType("urlSegment")]
        public string UrlSegment
        {
            get
            {
                var urlSegment = this.GetVortoValue<string>("urlSegment");

                if (string.IsNullOrEmpty(urlSegment))
                {
                    urlSegment = this.UrlName;
                }

                return urlSegment.ToUrlSegment().EnsureEndsWith("/");
            }
        }

        /// <summary>
        /// Gets the url segment for a specific culture.
        /// </summary>
        /// <param name="culture">
        /// The culture
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetUrlSegment(string culture)
        {
            var urlSegment = this.GetVortoValue<string>("urlSegment", culture);

            if (string.IsNullOrEmpty(urlSegment))
            {
                urlSegment = this.UrlName;
            }

            return urlSegment.ToUrlSegment().EnsureEndsWith("/");
        }
    }
}