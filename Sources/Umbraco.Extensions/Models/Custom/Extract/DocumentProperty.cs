namespace Umbraco.Extensions.Models.Custom.Extract
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// The document property.
    /// </summary>
    [XmlInclude(typeof(TextProperty))]
    [XmlInclude(typeof(TextArrayProperty))]
    [XmlInclude(typeof(SingleNestedContentProperty))]
    [XmlInclude(typeof(NestedContentProperty))]
    [XmlInclude(typeof(VortoContentProperty))]
    [XmlInclude(typeof(VortoStringProperty))]
    [Serializable]
    public class DocumentProperty
    {
        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        [XmlAttribute]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        [XmlAttribute]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the source language.
        /// </summary>
        [XmlIgnore]
        public string SourceLanguage { get; set; }

        /// <summary>
        /// Gets or sets the target language.
        /// </summary>
        [XmlIgnore]
        public string TargetLanguage { get; set; }
    }
}