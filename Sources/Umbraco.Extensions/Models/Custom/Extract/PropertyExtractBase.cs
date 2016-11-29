namespace Umbraco.Extensions.Models.Custom.Extract
{
    using System.Linq;
    using System.Text;

    using Umbraco.Core.Models;
    using Umbraco.Extensions.Extensions;
    using Umbraco.Web;

    /// <summary>
    /// The property extract base.
    /// </summary>
    public abstract class PropertyExtractBase
    {
        /// <summary>
        /// Gets the allowed types.
        /// </summary>
        public abstract string[] AllowedTypes { get; }

        /// <summary>
        /// Checks if the extract is for this property type.
        /// </summary>
        /// <param name="propertyTypeAlias">
        /// The property type alias.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsExtractForPropertyType(string propertyTypeAlias)
        {
            return this.AllowedTypes.Contains(propertyTypeAlias);
        }

        /// <summary>
        /// Converts the content into a string for Examine.
        /// </summary>
        /// /// 
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
        public virtual void Examine(
            StringBuilder extractedContent, 
            IPublishedContent content, 
            string alias, 
            string language = null)
        {
            var text = content.GetPropertyValue<string>(alias);
            if (!string.IsNullOrEmpty(text))
            {
                extractedContent.Append(" " + content.GetPropertyValue<string>(alias));
            }
        }

        /// <summary>
        /// The export.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="alias">
        /// The alias.
        /// </param>
        /// <param name="sourceLanguage">
        /// The source language.
        /// </param>
        /// <param name="label">
        /// The label.
        /// </param>
        /// <returns>
        /// The <see cref="DocumentProperty"/>.
        /// </returns>
        public virtual DocumentProperty Export(
            IPublishedContent content,
            string alias,
            string sourceLanguage,
            string label = "")
        {
            return new TextProperty
            {
                Alias = alias,
                Label = label,
                Value = content.GetPropertyValue<string>(alias).RemoveInvalidXmlChars()
            };
        }
    }
}