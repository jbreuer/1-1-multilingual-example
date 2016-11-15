namespace Umbraco.Extensions.Models.Custom
{
    using System.Collections.Generic;

    /// <summary>
    /// The url preview.
    /// </summary>
    public class UrlPreview
    {
        /// <summary>
        /// Gets or sets a value indicating whether is preview.
        /// </summary>
        public bool IsPreview { get; set; }

        /// <summary>
        /// Gets or sets the urls.
        /// </summary>
        public IEnumerable<string> Urls { get; set; }
    }
}