// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RichTextPropertyExtractItem.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The rich text property extract item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Umbraco.Extensions.Extract
{
    using System.Text;

    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Extensions.Models.Custom;
    using Umbraco.Web;

    /// <summary>
    /// The rich text property extract item.
    /// </summary>
    public class RichTextPropertyExtractItem : PropertyExtractBase
    {
        /// <summary>
        /// Gets the allowed types.
        /// </summary>
        public override string[] AllowedTypes
        {
            get
            {
                return new[] { Constants.PropertyEditors.TinyMCEAlias };
            }
        }

        /// <summary>
        /// Converts the content into a string for Examine.
        /// </summary>
        /// <param name="extractedContent">
        /// The extracted content.
        /// </param>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="alias">
        /// The alias.
        /// </param>
        /// <param name="language">
        /// The language.
        /// </param>
        public override void Examine(
            StringBuilder extractedContent, 
            IPublishedContent content, 
            string alias, 
            string language = null)
        {
            var text = content.GetPropertyValue<string>(alias);

            if (!string.IsNullOrEmpty(text))
            {
                extractedContent.Append(" " + text.StripHtml().StripNewLines());
            }
        }
    }
}