using AlgoCards.Enums;
using AlgoCards.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class BlackJack : ComponentBase
    {
        private Deck deck = new();
        private Queue<Card> drawPile = new();
        private int playerBank = 100;
        private int wager = 0;
        private bool handIsActive = false;
        private List<Card> playerHand = new();
        private List<Card> dealerHand = new();

        private string message;

        protected override async Task OnInitializedAsync()
        {
            playerBank = 100;
            wager = 0;
            await Task.CompletedTask;
        }

        private async Task Deal()
        {
            if (WagerIsValid())
            {
                await DealCards();
            }
        }

        private bool WagerIsValid()
        {
            if (wager < 5)
            {
                message = "Minimum wager value is 5";
                return false;
            }
            if (wager%5 != 0)
            {
                message = "Wager must be in increments of 5";
                return false;
            }
            
            return true;
        }

        private async Task DealCards()
        {
            message = "Dealing Cards";
            playerBank -= wager;
            handIsActive = true;
            
            deck = new Deck(DeckType.Shuffled);
            drawPile = new Queue<Card>(deck.NoJokers());
            playerHand = new List<Card>();
            dealerHand = new List<Card>();

            var burn = drawPile.Dequeue();
            playerHand.Add(drawPile.Dequeue());
            dealerHand.Add(drawPile.Dequeue());
            playerHand.Add(drawPile.Dequeue());
            dealerHand.Add(drawPile.Dequeue());

            await Task.CompletedTask;
        }

        private async Task EndHand()
        {
            handIsActive = false;
            await Task.CompletedTask;
        }
    }
}
