using Markdig;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Utils
{
    public static class MarkdownServices
    {
        public static async Task<string> GetContentFromUrl(HttpClient http, string fromUrl)
        {
            HttpResponseMessage httpResponse = await http.GetAsync(fromUrl);
            return httpResponse.IsSuccessStatusCode ?
                await httpResponse.Content.ReadAsStringAsync()
                : httpResponse.ReasonPhrase ?? "Error: Not found";
        }

        public static MarkupString GetMarkupString(string content)
        {
            var html = Markdig.Markdown.ToHtml(
                markdown: content,
                pipeline: new MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
            );

            return new MarkupString(html);
        }
    }
}
