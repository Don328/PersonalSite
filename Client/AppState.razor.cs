using BlazorApp.Client.Utils.Constants;
using BlazorApp.Client.Utils.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Client
{
    public partial class AppState : ComponentBase
    {
        private string selectedTheme 
            = ThemeOptions.liteMode;

        private bool acceptedTOS = false;

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
                SaveSelectedTheme(value);
            }
        }

        public bool AcceptedTOS 
        { 
            get
            {
                return acceptedTOS;
            }
            set
            {
                acceptedTOS = value;
                SaveTOSAcceptance(value);
            }
        }

        protected override void OnInitialized()
        {
            GetSelectedTheme();
            GetTOSAcceptance();
        }

        private void SaveSelectedTheme(string value)
        {
            JS.InvokeVoid(
                JsFunctions.SetSessionStorage,
                nameof(selectedTheme),
                value);
            StateHasChanged();
        }

        private void SaveTOSAcceptance(bool value)
        {
            JS.InvokeVoid(
                JsFunctions.SetSessionStorage,
                nameof(acceptedTOS),
                value.ToString());
        }

        private void GetSelectedTheme()
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

        private void GetTOSAcceptance()
        {
            var accepted = JS.Invoke<string>(
                JsFunctions.GetSessionStorage,
                nameof(acceptedTOS));

            if (string.IsNullOrEmpty(accepted) ||
                accepted.CompareTo("true") != 0)
            {
                acceptedTOS = false;
            }
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
