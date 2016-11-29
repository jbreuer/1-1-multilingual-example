namespace Umbraco.Extensions.Models.Custom.Extract
{
    using System;

    /// <summary>
    /// The nested document.
    /// </summary>
    [Serializable]
    public class NestedDocument
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        public Document Document { get; set; }
    }
}
