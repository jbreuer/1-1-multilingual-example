﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VortoPropertyExtractItem.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The Vorto property extract item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Umbraco.Extensions.Extract
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Our.Umbraco.Vorto.Extensions;

    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Extensions.Extensions;
    using Umbraco.Extensions.Models.Custom;
    using Umbraco.Extensions.Models.Custom.Extract;

    /// <summary>
    /// The Vorto property extract item.
    /// </summary>
    public class VortoPropertyExtractItem : PropertyExtractBase
    {
        /// <summary>
        /// Gets the allowed types.
        /// </summary>
        public override string[] AllowedTypes
        {
            get
            {
                return new[] { "Our.Umbraco.Vorto" };
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
            var vortoContent = content.GetVortoValue<IPublishedContent>(alias, language);
            
            if (vortoContent != null)
            {
                vortoContent.ExtractForExamine(extractedContent, language);
            }
            else
            {
                var vortoContents = content.GetVortoValue<List<IPublishedContent>>(alias, language);
                if (vortoContents != null && vortoContents.Any())
                {
                    vortoContents.ForEach(x => x.ExtractForExamine(extractedContent, language));
                }
                else
                {
                    var vortoString = content.GetVortoValue<string>(alias, language);
                    if (!string.IsNullOrEmpty(vortoString))
                    {
                        extractedContent.Append(" " + vortoString.StripHtml().StripNewLines());
                    }
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
            var vortoContent = content.GetVortoValue<IPublishedContent>(alias, sourceLanguage);

            if (vortoContent != null)
            {
                return new VortoContentProperty
                {
                    Alias = alias,
                    Document = vortoContent.ExtractForExport(sourceLanguage, label.RemoveAfterLast(" - "), true)
                };
            }

            var vortoString = content.GetVortoValue<string>(alias, sourceLanguage);

            if (vortoString != null)
            {
                return new VortoStringProperty
                {
                    Alias = alias,
                    Label = label,
                    Value = vortoString.RemoveInvalidXmlChars()
                };
            }

            return null;
        }
    }
}
