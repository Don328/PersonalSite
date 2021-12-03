using AlgoCards.Enums;
using AlgoCards.Models;
using BlazorApp.Client.Utils.Enums;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Pages
{
    public partial class CardGames : ComponentBase
    {
        private CardGameType GameType { get; set; }

        private void ShowRandomCards()
        {
            GameType = CardGameType.Random; 
        }
        
        private void ShowBlackJack()
        {
            GameType = CardGameType.Blackjack;
        }
    }
}
