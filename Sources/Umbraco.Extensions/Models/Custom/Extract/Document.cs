namespace Umbraco.Extensions.Models.Custom.Extract
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// The document.
    /// </summary>
    [Serializable]
    public class Document
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document()
        {
            this.Properties = new List<DocumentProperty>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [XmlAttribute]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [XmlAttribute]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public List<DocumentProperty> Properties { get; set; }
    }
}
