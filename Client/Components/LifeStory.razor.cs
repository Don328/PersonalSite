using Blazored.Modal;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class LifeStory : ComponentBase
    {
        [CascadingParameter]
        public BlazoredModalInstance?
            ModalInstance { get; set; }

        [Parameter, EditorRequired]
        public string Title
        { get; set; } = string.Empty;

        [Parameter, EditorRequired]
        public string FilePath
        { get; set; } = string.Empty;

        async void Close()
        {
            if (ModalInstance != null)
            {
                await ModalInstance.CloseAsync();
            }
        }
    }
}
