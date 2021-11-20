using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorApp.Client.Pages
{
    public partial class AboutMe : ComponentBase
    {
        private enum Section
        {
            Interests,
            Stories,
            None
        }

        private Section SelectedSection 
        { get; set; } = Section.None;
    }
}
