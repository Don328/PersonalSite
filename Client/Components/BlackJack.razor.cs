using AlgoCards.Enums;
using AlgoCards.Models;
using BlazorApp.Client.Utils.Enums;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Components
{
    public partial class BlackJack : ComponentBase
    {
        private Deck deck = new();
        private Queue<Card> drawPile = new();
        private int playerBank = 100;
        private int handWager = 0;
        private int playerWager = 5;
        private bool handIsActive = false;
        private List<Card> playerHand = new();
        private List<Card> dealerHand = new();
        private int playerHandValue = 0;
        private int dealerHandValue = 0;
        private int dealerShowValue = 0;
        private List<BlackJackPlayerActions> playerActions = new();

        private string dealerMessage;

        protected override async Task OnInitializedAsync()
        {
            playerBank = 100;
            playerWager = 5;
            await Task.CompletedTask;
        }

        private async Task Deal()
        {
            if (WagerIsValid())
            {
                handWager = playerWager;
                await DealCards();
            }
        }

        private bool WagerIsValid()
        {
            if (playerWager < 5)
            {
                dealerMessage = "Minimum wager value is 5";
                return false;
            }
            if (playerWager % 5 != 0)
            {
                dealerMessage = "Wager must be in increments of 5";
                return false;
            }

            return true;
        }

        private async Task DealCards()
        {
            dealerMessage = "Dealing Cards";
            await Task.Delay(500);
            dealerMessage = String.Empty;
            playerBank -= playerWager;

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
            await Task.Delay(500);
            CalculateDealerHand();
            await Task.Delay(500);
            CalculateDealerShowValue();

            await PlayHand();

            await Task.CompletedTask;
        }

        private async Task PlayHand()
        {
            CalculatePlayerHand();
            playerActions = new();

            if (playerHand.Count == 2)
            {
                if (dealerHandValue == 21)
                {
                    if (playerHandValue == 21)
                    {
                        dealerMessage = "Push";
                        playerBank += handWager;
                        handIsActive = false;
                        return;
                    }

                    dealerMessage = "Dealer has Black Jack";
                    handIsActive = false;
                    return;
                }
                
                if (playerHandValue == 21)
                {
                    dealerMessage = "BlackJack!";
                    playerBank += handWager * 3;
                    handIsActive = false;
                    return;
                }
            }

            switch (playerHandValue)
            {
                case 21:
                    await DrawDealerHand();
                    return;
                case > 21:
                    dealerMessage = "Player Busted";
                    handIsActive = false;
                    return;
                case < 21:
                    AddPlayerActions();
                    break;
            }
        }

        private void AddPlayerActions()
        {
            playerActions.Add(BlackJackPlayerActions.Hit);
            playerActions.Add(BlackJackPlayerActions.Stay);
            if (playerHand.Count == 2)
            {
                playerActions.Add(BlackJackPlayerActions.DoubleDown);
                if (playerHand[0].Rank == playerHand[1].Rank)
                {
                    playerActions.Add(BlackJackPlayerActions.Split);
                }
            }
        }

        private List<Card> GetDealerShow(List<Card> hand)
        {
            return hand.GetRange(1, hand.Count - 1);
        }

        private async Task PlayerStays()
        {
            await DrawDealerHand();
        }

        private async void PlayerHits()
        {
            playerHand.Add(drawPile.Dequeue());
            await Task.Delay(1000);
            CalculatePlayerHand();
            if (playerHandValue == 21)
            {
                await DrawDealerHand();
                return;
            }
            if (playerHandValue > 21)
            {
                EndHand();
                return;
            }

            await PlayHand();
        }

        private async Task PlayerDoubles()
        {
            playerBank -= handWager;
            handWager = handWager * 2;
            await Task.Delay(500);
            playerHand.Add(drawPile.Dequeue());
            CalculatePlayerHand();
            await DrawDealerHand();
        }

        private void PlayerSplits()
        {

        }

        private async Task DrawDealerHand()
        {
            handIsActive = false;

            if (dealerHandValue < 17)
            {
                dealerHand.Add(drawPile.Dequeue());
                CalculateDealerHand();
                if (IsBusted(dealerHandValue))
                {
                    DealerBusts();
                    return;
                }
                await Task.Delay(2000);
                await DrawDealerHand();
            }

            if (dealerHandValue == 17)
            {
                if (IsSoftSeventeen(dealerHand))
                {
                    dealerHand.Add(drawPile.Dequeue());
                    CalculateDealerHand();
                    if (IsBusted(dealerHandValue))
                    {
                        DealerBusts();
                        return;
                    }

                    await Task.Delay(1000);
                    await DrawDealerHand();
                }
            }

            EndHand();
        }

        private bool IsSoftSeventeen(List<Card> hand)
        {
            var aces = new List<Card>();
            var noAces = new List<Card>();

            foreach (var card in hand)
            {
                switch (card.Rank)
                {
                    case "A":
                        aces.Add(card);
                        break;
                    default:
                        noAces.Add(card);
                        break;
                }
            }

            if (!aces.Any())
            {
                return false;
            }

            var valueWithOneBigAce =
                CalculateHandValue(noAces)
                + (aces.Count - 1)
                + 11;

            if (IsBusted(valueWithOneBigAce))
            {
                return true;
            }

            return false;
        }

        private void EndHand()
        {
            if (playerHandValue > 21)
            {
                dealerMessage = "Busted";
                handIsActive = false;
                return;
            }

            if (playerHandValue > dealerHandValue)
            {
                dealerMessage = "You Win!";
                playerBank += handWager * 2;
            }

            if (playerHandValue == dealerHandValue)
            {
                dealerMessage = "Push";
                playerBank += handWager;
            }

            if (!IsBusted(dealerHandValue) &&
                playerHandValue < dealerHandValue)
            {
                dealerMessage = "Dealer Wins";
            }

            handIsActive = false;
        }

        private void CalculatePlayerHand()
        {
            playerHandValue = 0;
            playerHandValue = CalculateHandValue(playerHand);
            if (IsBusted(playerHandValue))
            {
                PlayerBusts();
            }
            StateHasChanged();
        }

        private bool IsBusted(int value)
        {
            if (value > 21)
            {
                return true;
            }

            return false;
        }


        private void PlayerBusts()
        {
            dealerMessage = "You Busted. Dealer wins";
            handIsActive = false;
        }

        private void DealerBusts()
        {
            dealerMessage = "Dealer Busted! You win!";
            playerBank += handWager * 2;
            handIsActive = false;
        }

        private void CalculateDealerHand()
        {
            dealerHandValue = 0;
            dealerHandValue = CalculateHandValue(dealerHand);
            StateHasChanged();
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
