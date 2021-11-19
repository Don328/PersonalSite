using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorApp.Client.Pages
{
    public partial class AboutMe : ComponentBase
    {
        private enum SelectedStory
        {
            FE,
            ProSe,
            None
        }

        private KeyValuePair<SelectedStory, string> selectedStory { get; set; }
            = new(SelectedStory.None, string.Empty);

    }
}
