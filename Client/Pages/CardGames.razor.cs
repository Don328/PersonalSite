using AlgoCards.Enums;
using AlgoCards.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Pages
{
    public partial class CardGames : ComponentBase
    {
        private Deck deck = new();

        protected override async Task OnInitializedAsync()
        {
            deck = new Deck(DeckType.Shuffled);
            await Task.CompletedTask;
        }
    }
}
