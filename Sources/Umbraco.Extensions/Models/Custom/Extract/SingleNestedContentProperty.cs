namespace Umbraco.Extensions.Models.Custom.Extract
{
    using System;

    /// <summary>
    /// The single nested content property.
    /// </summary>
    [Serializable]
    public class SingleNestedContentProperty : DocumentProperty
    {
        /// <summary>
        /// Gets or sets the nested document.
        /// </summary>
        public NestedDocument NestedDocument { get; set; }
    }
}