using AlgoCards.Enums;
using AlgoCards.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class RandomCards : ComponentBase
    {
        private Deck deck = new();
        
        protected override async Task OnInitializedAsync()
        {
            deck = new Deck(DeckType.Shuffled);
            await Task.CompletedTask;
        }
    }
}
