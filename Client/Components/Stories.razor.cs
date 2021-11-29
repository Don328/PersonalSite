using BlazorApp.Client.Utils;
using BlazorApp.Client.Utils.Enums;
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
            Fifty3Pilots,
            None
        }

        [CascadingParameter]
        public AppState AppState { get; set; }

        [CascadingParameter]
        public IModalService? Modal { get; set; }

        private KeyValuePair<StoryName, string> SelectedStory
        { get; set; } = new(StoryName.None, string.Empty);

        private ModalOptions? Options { get; set; }
            = new ModalOptions();

        private async Task OnStorySelected(StoryName story)
        {
            await GetStoryPath(story);
            var modalParams = await GetParams();

            SelectedStory = new KeyValuePair<StoryName, string>(
                story, SelectedStory.Value);
            
            StateHasChanged();

            
            Modal?.Show<LifeStory>("", parameters:modalParams, options:Options);
        }

        private async Task GetStoryPath(StoryName story)
        {
            var pathRoot = "/markdown/about-me/stories/";
            switch (story)
            {
                case StoryName.Fifty3Pilots:
                    Options = await GetOpts(ModalTypes.Scrollable);
                    SelectedStory = new(story, pathRoot + "fifty3pilots.md");
                    break;
                case StoryName.FE:
                    Options = await GetOpts(ModalTypes.Default);
                    SelectedStory = new(story, pathRoot + "fe.md");
                    break;
                case StoryName.ProSe:
                    Options = await GetOpts(ModalTypes.Scrollable);
                    SelectedStory = new(story, pathRoot + "pro-se.md");
                    break;
                default:
                    Options = await GetOpts(ModalTypes.Default);
                    SelectedStory = new(StoryName.None, String.Empty);
                    break;
            }
             
            await Task.CompletedTask;
        }

        private async Task<ModalOptions> GetOpts(ModalTypes modalType)
        {
            switch (modalType)
            {
                case ModalTypes.Default:
                    return ModalOptionsFactory
                        .GetOptions(ModalTypes.Default, AppState);
                case ModalTypes.Scrollable:
                    return ModalOptionsFactory
                        .GetOptions(ModalTypes.Scrollable, AppState);
                default:
                    return await Task
                        .FromResult(new ModalOptions());
            }
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
