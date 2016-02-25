namespace Umbraco.Extensions
{
    using Umbraco.Core;

    /// <summary>
    /// The global.
    /// </summary>
    public class Global : Web.UmbracoApplication
    {
        /// <summary>
        /// The get boot manager.
        /// </summary>
        /// <returns>
        /// The <see cref="IBootManager"/>.
        /// </returns>
        protected override IBootManager GetBootManager()
        {
            return new CustomBootManager(this);
        }
    }
}