namespace Umbraco.Extensions.Models.Custom.Extract
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The text array property.
    /// </summary>
    [Serializable]
    public class TextArrayProperty : DocumentProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextArrayProperty"/> class.
        /// </summary>
        public TextArrayProperty()
        {
            this.Values = new List<string>();
        }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        public List<string> Values { get; set; }
    }
}