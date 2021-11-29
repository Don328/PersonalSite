namespace BlazorApp.Client.Utils.Constants
{

    public static class ThemeOptions
    {
        private static readonly List<string> themes 
            = new List<string>()
            { "light-mode", "dark-mode" };

        public static List<string> Themes 
        { 
            get 
            {
                return themes;
            }
        }
            

    }
}
