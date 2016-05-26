namespace Umbraco.Extensions.Models
{
    using Our.Umbraco.Vorto.Extensions;

    using Umbraco.ModelsBuilder;

    /// <summary>
    /// The umb news item.
    /// </summary>
    public partial class Image
    {
        /// <summary>
        /// Gets the alt text. If it's empty return the image name.
        /// </summary>
        [ImplementPropertyType("alt")]
        public string Alt
        {
            get
            {
                var alt = this.GetVortoValue<string>("alt");
                return !string.IsNullOrEmpty(alt) ? alt : this.Name;
            }
        }
    }
}