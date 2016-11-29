// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtractExtensions.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The extract extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Umbraco.Extensions.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Xml;
    using System.Xml.Serialization;

    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Models;
    using Umbraco.Extensions.Extract;
    using Umbraco.Extensions.Models.Custom.Extract;

    /// <summary>
    /// The extract extensions.
    /// </summary>
    public static class ExtractExtensions
    {
        /// <summary>
        /// Extract the content to a document for export.
        /// </summary>
        /// <param name="publishedContent">
        /// The published content.
        /// </param>
        /// <param name="sourceLanguage">
        /// The source language.
        /// </param>
        /// <param name="label">
        /// The label.
        /// </param>
        /// <param name="isNested">
        /// This IPublishedContent is a nested element. For example Vorto or Nested Content.
        /// </param>
        /// <returns>
        /// The <see cref="Document"/>.
        /// </returns>
        public static Document ExtractForExport(this IPublishedContent publishedContent, string sourceLanguage, string label = "", bool isNested = false)
        {
            var document = new Document
            {
                Id = publishedContent.Id,
                Name = publishedContent.Name,
                Type = publishedContent.DocumentTypeAlias
            };

            // get descriptive labels for properties
            List<KeyValuePair<string, string>> propertyLabels = new List<KeyValuePair<string, string>>();

            foreach (
                var tab in
                    ApplicationContext.Current.Services.ContentTypeService.GetContentType(
                        publishedContent.DocumentTypeAlias).CompositionPropertyGroups)
            {
                var tabName = tab.Name;

                foreach (var property in tab.PropertyTypes)
                {
                    propertyLabels.Add(
                        new KeyValuePair<string, string>(
                            property.Alias,
                            !isNested ? (publishedContent.Name + " - " + tabName + " - " + property.Name) : property.Name));
                }
            }

            var translatableProperties =
                publishedContent.ContentType.PropertyTypes.Where(
                    x =>
                    PropertyExtractResolver.Current.PropertyExtractItems.Exists(
                        y => y.IsExtractForPropertyType(x.PropertyEditorAlias))).ToList();

            foreach (var prop in translatableProperties)
            {
                var extractItem =
                    PropertyExtractResolver.Current.PropertyExtractItems.FirstOrDefault(
                        x => x.IsExtractForPropertyType(prop.PropertyEditorAlias));

                if (extractItem != null)
                {
                    try
                    {
                        var propertyLabel = propertyLabels.Select(p => new { p.Key, p.Value }).FirstOrDefault(x => x.Key == prop.PropertyTypeAlias);

                        var value = string.Empty;

                        if (propertyLabel != null)
                        {
                            value = propertyLabel.Value;
                        }
                        else
                        {
                            value = publishedContent.Name + " - Generic properties - " + prop.PropertyTypeAlias;
                        }

                        if (!string.IsNullOrEmpty(label))
                        {
                            value = label + " - " + value;
                        }

                        var property = extractItem.Export(publishedContent, prop.PropertyTypeAlias, sourceLanguage, value);

                        if (property != null)
                        {
                            document.Properties.Add(property);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error<IPublishedContent>(ex.Message, ex);
                    }
                }
            }

            return document;
        }

        /// <summary>
        /// Extract the content to a string for Examine.
        /// </summary>
        /// <param name="publishedContent">
        /// The published content.
        /// </param>
        /// <param name="extractedContent">
        /// The extractedContent.
        /// </param>
        /// <param name="language">
        /// The language.
        /// </param>
        public static void ExtractForExamine(
            this IPublishedContent publishedContent, 
            StringBuilder extractedContent, 
            string language = null)
        {
            var extractProperties =
                publishedContent.ContentType.PropertyTypes.Where(
                    x =>
                    PropertyExtractResolver.Current.PropertyExtractItems.Exists(
                        y => y.IsExtractForPropertyType(x.PropertyEditorAlias))).ToList();

            foreach (var prop in extractProperties)
            {
                var extractItem =
                    PropertyExtractResolver.Current.PropertyExtractItems.FirstOrDefault(
                        x => x.IsExtractForPropertyType(prop.PropertyEditorAlias));

                if (extractItem != null)
                {
                    try
                    {
                        extractItem.Examine(extractedContent, publishedContent, prop.PropertyTypeAlias, language);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error<IPublishedContent>(ex.Message, ex);
                    }
                }
            }
        }

        /// <summary>
        /// The serialize object.
        /// </summary>
        /// <param name="objectToSerialize">
        /// The object to serialize.
        /// </param>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string SerializeObject<T>(this T objectToSerialize) where T : class
        {
            if (objectToSerialize == null)
            {
                return null;
            }

            var serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder(100);

            using (var xmlWriter = XmlWriter.Create(sb))
            {
                serializer.Serialize(xmlWriter, objectToSerialize);

                return sb.ToString();
            }
        }
    }
}