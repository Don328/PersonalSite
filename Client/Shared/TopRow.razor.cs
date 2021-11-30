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

        protected override async Task OnInitializedAsync()
        {
            modalOpts = ModalOptionsFactory.GetOptions(
                ModalTypes.Scrollable,
                AppState);

            await base.OnInitializedAsync();
        }
    }

}
