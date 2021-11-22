using BlazorApp.Client.Utils;
using BlazorApp.Client.Utils.Constants;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [CascadingParameter] public IModalService? Modal { get; set; }

        private ModalOptions ModalOpts
        {
            get
            {
                return ModalOptionsFactory
                    .GetOptions(ModalTypes.Default);
            }
        }
    }
}
