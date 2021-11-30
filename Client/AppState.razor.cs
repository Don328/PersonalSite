using BlazorApp.Client.Utils.Constants;
using BlazorApp.Client.Utils.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Client
{
    public partial class AppState : ComponentBase
    {
        private string selectedTheme { get; set; }
            = ThemeOptions.liteMode;

        [Inject]
        public IJSInProcessRuntime JS { get; set; }

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
                JS.InvokeVoid(
                    JsFunctions.SetSessionStorage,
                    nameof(selectedTheme),
                    value);
                StateHasChanged();
            }
        }

        protected override void OnInitialized()
        {
            var option = JS.Invoke<string>(
                JsFunctions.GetSessionStorage,
                nameof(selectedTheme));
            if (string.IsNullOrEmpty(option))
            {
                option = ThemeOptions.liteMode;
            }

            selectedTheme = option;
        }

        public void ToggleTheme()
        {
            if (SelectedTheme == ThemeOptions.liteMode)
            {
                SelectedTheme = ThemeOptions.darkMode;

                return;
            }

            SelectedTheme = ThemeOptions.liteMode;
        }

        public bool IsSelectedTheme(string theme)
        {
            return theme == SelectedTheme;
        }
    }
}
