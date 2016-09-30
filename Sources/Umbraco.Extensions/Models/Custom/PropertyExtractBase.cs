// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyExtractBase.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The property extract base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Umbraco.Extensions.Models.Custom
{
    using System.Linq;
    using System.Text;

    using Umbraco.Core.Models;
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
    }
}