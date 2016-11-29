// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextPropertyExtractItem.cs" company="Colours B.V.">
//   © Colours B.V. 2015
// </copyright>
// <summary>
//   The text property extract item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Umbraco.Extensions.Extract
{
    using Umbraco.Core;
    using Umbraco.Extensions.Models.Custom;
    using Umbraco.Extensions.Models.Custom.Extract;

    /// <summary>
    /// The text property extract item.
    /// </summary>
    public class TextPropertyExtractItem : PropertyExtractBase
    {
        /// <summary>
        /// Gets the allowed types.
        /// </summary>
        public override string[] AllowedTypes
        {
            get
            {
                return new[] { Constants.PropertyEditors.TextboxAlias, Constants.PropertyEditors.TextboxMultipleAlias };
            }
        }
    }
}