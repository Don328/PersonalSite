using BlazorApp.Client.Components;
using BlazorApp.Client.Utils;
using BlazorApp.Client.Utils.Enums;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Shared
{
    public partial class TopRow : ComponentBase
    {
        private ModalOptions modalOpts = new();

        [CascadingParameter]
        public AppState AppState { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        private void ShowModal()
        {
            modalOpts = ModalOptionsFactory.GetOptions(
                ModalTypes.Scrollable,
                AppState);

            Modal.Show<About>(string.Empty, modalOpts);
        }
    }

}
