namespace Umbraco.Extensions.Models.Custom.Extract
{
    using System;

    /// <summary>
    /// The property.
    /// </summary>
    [Serializable]
    public class TextProperty : DocumentProperty
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }
    }
}