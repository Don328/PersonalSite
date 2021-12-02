using AlgoCards.Enums;
using AlgoCards.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class PlayingCard : ComponentBase
    {
        [Parameter]
        public Card Card { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.CompletedTask;
        }

    }
}
