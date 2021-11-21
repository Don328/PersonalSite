using Microsoft.AspNetCore.Components;
using Markdig;
using Microsoft.AspNetCore.Html;

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
                Content = String.IsNullOrEmpty(FromUrl) ?
                    "Content or FromUrl property is not set or is invalid" 
                    : await InitContentFromUrl();
        }

        private async Task<string> InitContentFromUrl()
        {
            HttpResponseMessage httpResponse = await Http.GetAsync(FromUrl);
            return httpResponse.IsSuccessStatusCode ?
                await httpResponse.Content.ReadAsStringAsync() 
                : httpResponse.ReasonPhrase ?? "Error: Not found";
        }

        private MarkupString RenderAsHtml()
        {
            var html = Markdig.Markdown.ToHtml(
                markdown: Content,
                pipeline: new MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
            );

            return new MarkupString(html);
        }
    }
}
