namespace Umbraco.Extensions.Extract
{
    /// <summary>
    /// The translate menu action.
    /// </summary>
    public class ExtractMenuItem : umbraco.interfaces.IAction
    {
        private static ExtractMenuItem instance;

        internal ExtractMenuItem()
        {
            this.CanBePermissionAssigned = true;
            this.ShowInNotifier = false;
            this.Alias = "ExtractMenuAction";
            this.Icon = "alarm-clock"; // css class of icon
            this.JsFunctionName = string.Empty;
            this.JsSource = string.Empty;
            this.Letter = ' ';
            instance = this;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static ExtractMenuItem Instance
        {
            get
            {
                return instance ?? new ExtractMenuItem();
            }
        }

        /// <summary>
        /// Gets the letter.
        /// </summary>
        public char Letter { get; private set; }

        /// <summary>
        /// Gets a value indicating whether show in notifier.
        /// </summary>
        public bool ShowInNotifier { get; private set; }

        /// <summary>
        /// Gets a value indicating whether can be permission assigned.
        /// </summary>
        public bool CanBePermissionAssigned { get; private set; }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        public string Icon { get; private set; }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        public string Alias { get; private set; }

        /// <summary>
        /// Gets the js function name.
        /// </summary>
        public string JsFunctionName { get; private set; }

        /// <summary>
        /// Gets A path to a supporting JavaScript file for the IAction. A script tag will be rendered out with the reference to the JavaScript file.
        /// </summary>
        public string JsSource { get; private set; }
    }
}