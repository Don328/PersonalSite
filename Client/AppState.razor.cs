using BlazorApp.Client.Utils.Constants;
using BlazorApp.Client.Utils.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Client
{
    public partial class AppState : ComponentBase
    {
        private Theme theme = Theme.dark;

        [Inject]
        public IJSInProcessRuntime JS { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }


        public Theme Theme
        {
            get
            {
                return theme;
            }
            set
            {
                theme = value;

                JS.InvokeVoid(
                    JsFunctions.ToggleTheme,
                    value.ToString());

                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await JS.InvokeVoidAsync(
                JsFunctions.ToggleTheme,
                theme.ToString());
        }

        public async Task ToggleTheme()
        {
            switch (theme)
            {
                case Theme.light:
                    theme = Theme.dark;
                    break;
                case Theme.dark:
                    theme = Theme.light;
                    break;
                default:
                    theme = Theme.light;
                    break;
            }

            await JS.InvokeVoidAsync(
                JsFunctions.ToggleTheme,
                theme.ToString());

                StateHasChanged();
        }
    }

}
