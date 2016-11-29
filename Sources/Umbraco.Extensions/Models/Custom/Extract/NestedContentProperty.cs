namespace Umbraco.Extensions.Models.Custom.Extract
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The nested content property.
    /// </summary>
    [Serializable]
    public class NestedContentProperty : DocumentProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NestedContentProperty"/> class.
        /// </summary>
        public NestedContentProperty()
        {
            this.NestedDocuments = new List<NestedDocument>();
        }

        /// <summary>
        /// Gets or sets the nested documents.
        /// </summary>
        public List<NestedDocument> NestedDocuments { get; set; }
    }
}