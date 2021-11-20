using BlazorApp.Client.Utils;
using BlazorApp.Client.Utils.Constants;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class Stories : ComponentBase
    {
        private enum StoryName
        {
            FE,
            ProSe,
            None
        }

        [CascadingParameter]
        public IModalService? Modal { get; set; }

        private KeyValuePair<StoryName, string> SelectedStory
        { get; set; } = new(StoryName.None, string.Empty);

        private ModalOptions ModalOpts
        {
            get
            {
                return ModalOptionsFactory
                    .GetOptions(ModalTypes.Default);
            }
        }

        private async Task OnStorySelected(StoryName story)
        {
            await GetStoryPath(story);
            var modalParams = await GetParams();

            SelectedStory = new KeyValuePair<StoryName, string>(
                story, SelectedStory.Value);
            
            StateHasChanged();
            

            Modal?.Show<LifeStory>(story.ToString(), parameters:modalParams, options:ModalOpts);
        }

        private async Task GetStoryPath(StoryName story)
        {
            var pathRoot = "/markdown/about-me/stories/";
            switch (story)
            {
                case StoryName.FE:
                    SelectedStory = new(story, pathRoot + "fe.md");
                    break;
                case StoryName.ProSe:
                    SelectedStory = new(story, pathRoot + "pro-se.md");
                    break;
                default:
                    SelectedStory = new(StoryName.None, String.Empty);
                    break;
            }
             
            await Task.CompletedTask;
        }

        private async Task<ModalParameters> GetParams()
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(
                LifeStory.Title), SelectedStory.Key.ToString());
            parameters.Add(nameof(
                LifeStory.FilePath), SelectedStory.Value);
            
            return await Task.FromResult(parameters);
        }
    }
}
