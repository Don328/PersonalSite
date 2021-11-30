namespace BlazorApp.Client.Utils.Constants
{
    public static class JsFunctions
    {
        // js function constants for theme toggle
        private const string ThemeTogglePrefix = "theme";
        public const string ToggleTheme =
            $"{ThemeTogglePrefix}.toggle";

        // js function constants for session store
        private const string SessionStorePrefix = "stateStore";
        public const string GetSessionStorage =
            $"{SessionStorePrefix}.getSessionStorage";
        public const string SetSessionStorage =
            $"{SessionStorePrefix}.setSessionStorage";
    }
}
