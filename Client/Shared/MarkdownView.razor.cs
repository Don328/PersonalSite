using Microsoft.AspNetCore.Components;
using Markdig;
using Microsoft.AspNetCore.Html;
using BlazorApp.Client.Utils;

namespace BlazorApp.Client.Shared
{
    public partial class MarkdownView : ComponentBase
    {
        [Inject]
        HttpClient Http { get; set; } = new();

        [Parameter]
        public string Content { get; set; } = string.Empty;
        
        [Parameter]
        public string FromUrl { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (string.IsNullOrEmpty(Content))
            {
                Content = await MarkdownServices
                      .GetContentFromUrl(Http, FromUrl);
            }
        }

        private MarkupString RenderAsHtml()
        {
            return MarkdownServices.GetMarkupString(Content);
        }
    }
}
