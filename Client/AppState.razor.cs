using BlazorApp.Client.Utils.Constants;
using BlazorApp.Client.Utils.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Client
{
    public partial class AppState : ComponentBase
    {
        private string selectedTheme { get; set; }
            = "light-mode";

        [Parameter]
        public RenderFragment ChildContent { get; set; }


        public string SelectedTheme
        {
            get
            {
                return selectedTheme;
            }
            private set
            {
                selectedTheme = value;
            }
        }

        public void ToggleTheme()
        {
            if (SelectedTheme == "light-mode")
            {
                SelectedTheme = "dark-mode";

                return;
            }

            SelectedTheme = "light-mode";
        }

        public bool IsSelectedTheme(string theme)
        {
            return theme == SelectedTheme;
        }
    }
}
