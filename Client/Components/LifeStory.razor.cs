using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class LifeStory : ComponentBase
    {
        [Parameter, EditorRequired]
        public string Title
        { get; set; } = string.Empty;
        
        [Parameter, EditorRequired]
        public string FilePath
        { get; set; } = string.Empty;
    }
}
