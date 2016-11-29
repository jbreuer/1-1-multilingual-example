// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NestedContentExtractItem.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The nested content property extract item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Umbraco.Extensions.Extract
{
    using System.Collections.Generic;
    using System.Text;

    using Umbraco.Core.Models;
    using Umbraco.Extensions.Extensions;
    using Umbraco.Extensions.Models.Custom;
    using Umbraco.Extensions.Models.Custom.Extract;
    using Umbraco.Web;

    /// <summary>
    /// The nested content property extract item.
    /// </summary>
    public class NestedContentExtractItem : PropertyExtractBase
    {
        /// <summary>
        /// Gets the allowed types.
        /// </summary>
        public override string[] AllowedTypes
        {
            get
            {
                return new[] { "Our.Umbraco.NestedContent" };
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
            var multipleContent = content.GetPropertyValue<IEnumerable<IPublishedContent>>(alias);

            if (multipleContent != null)
            {
                foreach (var publishedContent in multipleContent)
                {
                    publishedContent.ExtractForExamine(extractedContent);
                }
            }
            else
            {
                var singleContent = content.GetPropertyValue<IPublishedContent>(alias);

                if (singleContent != null)
                {
                    singleContent.ExtractForExamine(extractedContent);
                }
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
        public override DocumentProperty Export(IPublishedContent content, string alias, string sourceLanguage, string label = "")
        {
            var multipleContent = content.GetPropertyValue<IEnumerable<IPublishedContent>>(alias);

            if (multipleContent != null)
            {
                var property = new NestedContentProperty { Alias = alias };

                foreach (var publishedContent in multipleContent)
                {
                    property.NestedDocuments.Add(
                        new NestedDocument
                        {
                            Name = publishedContent.Name,
                            Document = publishedContent.ExtractForExport(sourceLanguage, label.RemoveAfterLast(" - "), true)
                        });
                }

                return property;
            }

            var singleContent = content.GetPropertyValue<IPublishedContent>(alias);

            if (singleContent != null)
            {
                return new SingleNestedContentProperty
                {
                    Alias = alias,
                    Label = label,
                    NestedDocument = new NestedDocument
                    {
                        Name = singleContent.Name,
                        Document = singleContent.ExtractForExport(sourceLanguage)
                    }
                };
            }

            return null;
        }
    }
}