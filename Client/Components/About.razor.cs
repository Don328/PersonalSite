using Blazored.Modal;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class About : ComponentBase
    {
        [CascadingParameter]
        BlazoredModalInstance? ModalInstance { get; set; }

        async void Close()
        {
            if (ModalInstance != null)
            {
                await ModalInstance.CloseAsync();
            }
        }

    }
}
