namespace Umbraco.Extensions.Models.Custom.Extract
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// The document collection.
    /// </summary>
    [Serializable]
    public class DocumentCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentCollection"/> class.
        /// </summary>
        public DocumentCollection()
        {
            this.Documents = new List<Document>();
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
        /// Gets or sets the source language.
        /// </summary>
        [XmlAttribute]
        public string SourceLanguage { get; set; }

        /// <summary>
        /// Gets or sets the target language.
        /// </summary>
        [XmlAttribute]
        public string TargetLanguage { get; set; }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        public List<Document> Documents { get; set; }
    }
}