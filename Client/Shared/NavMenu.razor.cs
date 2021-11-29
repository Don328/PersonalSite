using BlazorApp.Client.Utils;
using BlazorApp.Client.Utils.Enums;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Shared
{
    public partial class NavMenu : ComponentBase
    {
        [CascadingParameter] public IModalService? Modal { get; set; }

        private bool collapseNavMenu = true;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        private ModalOptions ModalOpts
        {
            get
            {
                return ModalOptionsFactory
                    .GetOptions(ModalTypes.Scrollable);
            }
        }
    }
}