using BlazorApp.Client.Utils;
using BlazorApp.Client.Utils.Enums;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        private List<string> themes =
            new List<string> 
            { "light-mode", "dark-mode" };
        
        private string selectedTheme = "light-mode";

        [CascadingParameter] public IModalService? Modal { get; set; }

        private ModalOptions ModalOpts
        {
            get
            {
                return ModalOptionsFactory
                    .GetOptions(ModalTypes.Scrollable);
            }
        }

        private string GetTheme()
        {
            return selectedTheme;
        }

        private void ToggleTheme()
        {
            if (selectedTheme == "light-mode")
            {
                selectedTheme = "dark-mode";

                StateHasChanged();
                return;
            }

            selectedTheme = "light-mode";

            StateHasChanged();
        }

        private bool IsSelectedTheme(string theme)
        {
            return theme == selectedTheme;
        }
    }
}
