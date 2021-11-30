namespace BlazorApp.Client.Utils.Constants
{

    public static class ThemeOptions
    {
        public const string liteMode = "light-mode";
        public const string darkMode = "dark-mode";

        private static readonly List<string> themes 
            = new List<string>()
            { liteMode, darkMode };

        public static List<string> Themes 
        { 
            get 
            {
                return themes;
            }
        }


    }
}
