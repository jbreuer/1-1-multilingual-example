namespace Umbraco.Extensions.Models.Custom.Extract
{
    using System;

    /// <summary>
    /// The VORTO content property.
    /// </summary>
    [Serializable]
    public class VortoContentProperty : DocumentProperty
    {
        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        public Document Document { get; set; }
    }
}