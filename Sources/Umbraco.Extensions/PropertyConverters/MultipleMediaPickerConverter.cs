using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Extensions.PropertyConverters
{
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Extensions.Models;

    using UmbracoContext = Umbraco.Web.UmbracoContext;

    public class MultipleMediaPickerConverter : PropertyValueConverterBase, IPropertyValueConverterMeta
    {
        /// <summary>
        /// Convert source to destination object.
        /// </summary>
        /// <param name="propertyType">
        /// Property type.
        /// </param>
        /// <param name="source">
        /// Source object.
        /// </param>
        /// <param name="preview">
        /// Preview.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null)
            {
                return null;
            }

            var nodeIds =
                source.ToString()
                    .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x =>
                    {
                        int id = int.TryParse(x, out id) ? id : -1;
                        return id;
                    })
                    .ToList();

            if (IsMultipleDataType(propertyType.DataTypeId))
            {
                if (!IsConverterDefault(propertyType))
                {
                    return nodeIds.Select(x =>
                    {
                        var image = x < 0 ? null : UmbracoContext.Current.MediaCache.GetById(x) as Image;
                        return image;
                    }).Where(x => x != null).ToList();
                }

                return nodeIds.Select(x =>
                {
                    var media = x < 0 ? null : UmbracoContext.Current.MediaCache.GetById(x);
                    return media;
                }).Where(x => x != null).ToList();
            }

            if (nodeIds.Any())
            {
                var id = nodeIds.First();
                if (!IsConverterDefault(propertyType))
                {
                    var image = id < 0 ? null : UmbracoContext.Current.MediaCache.GetById(id) as Image;
                    if (image != null)
                    {
                        return image;
                    }
                }

                var media = id < 0 ? null : UmbracoContext.Current.MediaCache.GetById(id);
                return media;
            }

            return null;
        }

        /// <summary>
        /// Override for IsConverter.
        /// </summary>
        /// <param name="propertyType">
        /// Property type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return Constants.PropertyEditors.MultipleMediaPickerAlias.InvariantEquals(propertyType.PropertyEditorAlias);
        }

        /// <summary>
        /// Gets property cache level.
        /// </summary>
        /// <param name="propertyType">
        /// Property type.
        /// </param>
        /// <param name="cacheValue">
        /// Cache value.
        /// </param>
        /// <returns>
        /// The <see cref="PropertyCacheLevel"/>.
        /// </returns>
        public PropertyCacheLevel GetPropertyCacheLevel(PublishedPropertyType propertyType, PropertyCacheValue cacheValue)
        {
            return PropertyCacheLevel.ContentCache;
        }

        /// <summary>
        /// Gets property value type.
        /// </summary>
        /// <param name="propertyType">
        /// Property type.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        public Type GetPropertyValueType(PublishedPropertyType propertyType)
        {
            if (IsMultipleDataType(propertyType.DataTypeId))
            {
                return !IsConverterDefault(propertyType) ? typeof(IEnumerable<Image>) : typeof(IEnumerable<IPublishedContent>);
            }

            return !IsConverterDefault(propertyType) ? typeof(Image) : typeof(IPublishedContent);
        }

        /// <summary>
        /// Almost all media picker properties are images so here we exclude the properties that aren't.
        /// </summary>
        /// <param name="propertyType">
        /// Property type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsConverterDefault(PublishedPropertyType propertyType)
        {
            return true;
        }

        /// <summary>
        /// Check if the multi picker checkbox is checked.
        /// </summary>
        /// <param name="dataTypeId">
        /// Data type id
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsMultipleDataType(int dataTypeId)
        {
            var dts = ApplicationContext.Current.Services.DataTypeService;
            var multiPickerPreValue =
                dts.GetPreValuesCollectionByDataTypeId(dataTypeId)
                    .PreValuesAsDictionary.FirstOrDefault(
                        x => x.Key.InvariantEquals("multiPicker")).Value;

            return multiPickerPreValue != null && multiPickerPreValue.Value.TryConvertTo<bool>().Result;
        }
    }
}
