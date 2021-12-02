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
        private int playerHandValue = 0;
        private int dealerHandValue = 0;
        private int dealerShowValue = 0;

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
            
            deck = new Deck(DeckType.Shuffled);
            drawPile = new Queue<Card>(deck.NoJokers());
            playerHand = new List<Card>();
            dealerHand = new List<Card>();

            handIsActive = true;
            var burn = drawPile.Dequeue();
            playerHand.Add(drawPile.Dequeue());
            dealerHand.Add(drawPile.Dequeue());
            playerHand.Add(drawPile.Dequeue());
            dealerHand.Add(drawPile.Dequeue());
            CalculateDealerHand();
            CalculatePlayerHand();
            CalculateDealerShowValue();
            
            await Task.CompletedTask;
        }

        private List<Card> GetDealerShow(List<Card> hand)
        {
            return hand.GetRange(1, hand.Count -1);
        }

        private async Task EndHand()
        {
            handIsActive = false;
            await Task.CompletedTask;
        }

        private void CalculatePlayerHand()
        {
            playerHandValue = 0;
            playerHandValue = CalculateHandValue(playerHand);
        }

        private void CalculateDealerHand()
        {
            dealerHandValue = 0;
            dealerHandValue = CalculateHandValue(dealerHand);
        }

        private int CalculateHandValue(List<Card> cards)
        {
            var value = 0;
            List<Card> aces = new();
            foreach (var card in cards)
            {
                switch (card.CardValue)
                {
                    default:
                        value += card.CardValue;
                        break;
                    case > 9:
                        value += 10;
                        break;
                    case 1:
                        aces.Add(card);
                        break;
                }
            }

            foreach (var ace in aces)
            {
                switch (value)
                {
                    default:
                        value += 11;
                        break;
                    case > 10:
                        value += 1;
                        break;
                }
            }

            return value;
        }

        private void CalculateDealerShowValue()
        {
            dealerShowValue = 0;
            foreach (var card in GetDealerShow(dealerHand))
            {
                switch (card.CardValue)
                {
                    default:
                        dealerShowValue += card.CardValue;
                        break;
                    case 1:
                        dealerShowValue += 11;
                        break;
                    case > 9:
                        dealerShowValue += 10;
                        break;
                }
            }
        }
    }
}
