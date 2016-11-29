﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextArrayPropertyExtractItem.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The text array property extract item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Umbraco.Extensions.Extract
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Extensions.Extensions;
    using Umbraco.Extensions.Models.Custom;
    using Umbraco.Extensions.Models.Custom.Extract;
    using Umbraco.Web;

    /// <summary>
    /// The text array property extract item.
    /// </summary>
    public class TextArrayPropertyExtractItem : PropertyExtractBase
    {
        /// <summary>
        /// Gets the allowed types.
        /// </summary>
        public override string[] AllowedTypes
        {
            get
            {
                return new[] { Constants.PropertyEditors.MultipleTextstringAlias };
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
            var values = content.GetPropertyValue<IEnumerable<string>>(alias);
            var list = new List<string>();

            if (values != null)
            {
                // GetPropertyValue returns collection of property values with empty strings between values
                list = values.Where(x => x != string.Empty).ToList();
            }

            if (list.Any())
            {
                extractedContent.Append(" " + string.Join(" ", list));
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
        public override DocumentProperty Export(
            IPublishedContent content,
            string alias,
            string sourceLanguage,
            string label = "")
        {
            var values = content.GetPropertyValue<IEnumerable<string>>(alias);
            var list = new List<string>();

            if (values != null)
            {
                list = values.Where(x => x != string.Empty).Select(x => x.RemoveInvalidXmlChars()).ToList();
            }

            return new TextArrayProperty { Alias = alias, Label = label, Values = list };
        }
    }
}