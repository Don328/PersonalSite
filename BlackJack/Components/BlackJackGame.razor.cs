using AlgoCards.Enums;
using AlgoCards.Models;
using BlackJack.Enums;
using BlackJack.Models;
using BlackJack.Utils;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Components
{
    public partial class BlackJackGame : ComponentBase
    {
        private bool showDealButton = true;
        private bool showDealerHand = false;
        private Player player = new(200);
        private DealerHand dealerHand = new();
        private string dealerMessage = string.Empty;
        private Deck deck = new();
        private Queue<Card> drawPile = new();
        private List<PlayerActions> playerActions = new();
        private bool showPlayerActions = false;
        private bool handIsPaid = false;
        private PlayerHand currentHand = new(10);

        protected override async Task OnInitializedAsync()
        {
            player = new Player(200);
            player.Wager = 10;

            await Task.CompletedTask;
        }

        private async Task NewHand()
        {
            var validator = WagerValidator.IsValid(player.Wager);

            if (player.HasEnoughBank
                && validator 
                == WagerValidator.successMessage)
            {
                await SetupHand();
                await DealCards();

                await Task.Delay(500);
                if (await CheckDealerBlackJack())
                {
                    showDealerHand = true;
                    EndHand();
                    return;
                }

                await WaitForActions();

                if (!player.IsActive)
                {
                    await PlayDealerHand();
                }
            }
        }

        private async Task SetupHand()
        {
            SetupDeck();
            SetupDealer();
            await SetupPlayer();
        }

        private async Task SetupPlayer()
        {
            await Task.Delay(500);
            player.Hands.Clear();
            currentHand = new(player.Wager);
            player.Bank -= player.Wager;
            await Task.Delay(500);
            dealerMessage = String.Empty;
            handIsPaid = false;
        }

        private void SetupDealer()
        {
            showDealerHand = false;
            dealerHand.Cards.Clear();
            dealerHand = new();
            showDealButton = false;
        }

        private void SetupDeck()
        {
            deck.Cards.Clear();
            deck = new Deck(DeckType.Shuffled);
            drawPile.Clear();
            drawPile = new Queue<Card>(deck.NoJokers());
            dealerMessage = "Dealing Cards";
        }

        private async Task DealCards()
        {
            player.Hands.Clear();
            player.Hands.Add(new PlayerHand(player.Wager));
            dealerHand = new DealerHand();
            var burn = drawPile.Dequeue();
            await player.Hands[0].AddCard(drawPile.Dequeue());
            StateHasChanged();
            await Task.Delay(500);

            await dealerHand.AddCard(drawPile.Dequeue());
            StateHasChanged();
            await Task.Delay(500);

            await player.Hands[0].AddCard(drawPile.Dequeue());
            StateHasChanged();
            await Task.Delay(500);

            await dealerHand.AddCard(drawPile.Dequeue());
            StateHasChanged();
            await Task.Delay(500);
        }

        private async Task<bool> CheckDealerBlackJack()
        {
            if (dealerHand.Value == 21)
            {
                return await Task.FromResult(true);
            }
            
            return await Task.FromResult(false);
        }

        private async Task WaitForActions()
        {
            var activeHands = (
                from h in player.Hands
                where h.Stayed == false
                && h.IsBusted == false
                && h.Value != 21
                select h).ToList();

            if (activeHands.Any())
            {
                showPlayerActions = true;
                
                foreach (var hand in activeHands)
                {
                    
                    currentHand = hand;
                    if (await PlayerCanAct())
                    {
                        dealerMessage = "Player's Move";
                    }
                }
            }
            else
            {
                await PlayDealerHand();
                return;
            }

            await Task.CompletedTask;
        }

        private async Task<bool> PlayerCanAct()
        {
                if (currentHand != null &&
                    !currentHand.IsBusted)
                {
                    playerActions.Clear();
                    StateHasChanged();

                    if (currentHand.Value == 21)
                    {
                        currentHand.Stay();
                        StateHasChanged();
                        return await Task.FromResult(true);
                    }

                    AddPlayerActions();
                    return await Task.FromResult(true);
                }

                dealerMessage = string.Empty;
                StateHasChanged();
                return await Task.FromResult(false);
        }

        private void AddPlayerActions()
        {
            playerActions.Clear();
            playerActions.Add(PlayerActions.Hit);
            playerActions.Add(PlayerActions.Stay);
            if (currentHand != null
                && currentHand.IsFirstMove)
            {
                playerActions.Add(PlayerActions.DoubleDown);
                if (currentHand != null
                    && currentHand.IsPair)
                {
                    playerActions.Add(PlayerActions.Split);
                }
            }
            StateHasChanged();
        }

        private async Task PlayerStays()
        {
            showPlayerActions = false;
            currentHand.Stay();
            StateHasChanged();

            await WaitForActions();

            if (!player.IsActive)
            {
                await PlayDealerHand();
            }
            
        }

        private async void PlayerHits()
        {
            showPlayerActions = false;
            await currentHand.AddCard(drawPile.Dequeue());
            StateHasChanged();

            if (currentHand.IsBusted)
            {
                currentHand.SetDealerMessage("Busted");
                if (player.Hands.IndexOf(currentHand) 
                    < player.Hands.Count -1)
                {
                    await WaitForActions();
                }
                else
                {
                    await PlayDealerHand();
                }
            }
            else
            {
                showPlayerActions=true;
                await WaitForActions();
            }

        }

        private async Task PlayerDoubles()
        {
            showPlayerActions = false;
            await Task.Delay(500);
            player.Bank -= currentHand.Wager;
            await currentHand.DoubleDown(drawPile.Dequeue());
            StateHasChanged();
            
            await WaitForActions();
            if (!player.IsActive)
            {
                await PlayDealerHand();
            }

        }

        private void PlayerSplits()
        {

        }

        private async Task PlayDealerHand()
        {
            showPlayerActions = false;
            dealerMessage = String.Empty;
            showDealerHand = true;

            var playerAlive = (
                from h in player.Hands
                where h.IsBusted == false
                && h.IsBlackJack == false
                select h).Any();
                
            if (!playerAlive)
            {
                EndHand();
                return;
            }

            dealerMessage = "Dealer's Move";
            StateHasChanged();
            if (dealerHand.CheckForHit())
            {
                await Task.Delay(500);
                await dealerHand.AddCard(drawPile.Dequeue());
                StateHasChanged();
                await PlayDealerHand();
            }
            dealerMessage = string.Empty;
            StateHasChanged();
            EndHand();
        }

        private void EndHand()
        {
            if (!handIsPaid)
            {
                dealerMessage = String.Empty;

                foreach (var hand in player.Hands)
                {
                    if (hand.Value > 21)
                    {
                        dealerMessage = "Busted!";
                    }
                    else if (hand.Value == dealerHand.Value)
                    {
                        dealerMessage = "Push";
                        player.Bank += hand.Wager;
                    }
                    else if (hand.Cards.Count == 2
                            && hand.Value == 21)
                    {
                        dealerMessage = "BlackJack!";
                        player.Bank += (hand.Wager / 10) * 25;
                    }
                    else if (dealerHand.Value > 21)
                    {
                        dealerMessage = $"Dealer busts! You win ${hand.Wager}";
                        player.Bank += hand.Wager * 2;
                    }
                    else if (hand.Value > dealerHand.Value)
                    {
                        dealerMessage = $"You win {hand.Wager}!";
                        player.Bank += hand.Wager * 2;
                    }
                    else
                    {
                        dealerMessage = "You lose.";
                    }

                    handIsPaid=true;
                    showDealButton = true;
                    StateHasChanged();
                }
            }
        }

        //private void CalculateDealerHand()
        //{
        //    dealerHandValue = 0;
        //    dealerHandValue = CalculateHandValue(dealerHand);
        //    StateHasChanged();
        //}

        //private int CalculateHandValue(List<Card> cards)
        //{
        //    var value = 0;
        //    List<Card> aces = new();
        //    foreach (var card in cards)
        //    {
        //        switch (card.CardValue)
        //        {
        //            default:
        //                value += card.CardValue;
        //                break;
        //            case > 9:
        //                value += 10;
        //                break;
        //            case 1:
        //                aces.Add(card);
        //                break;
        //        }
        //    }

        //    foreach (var ace in aces)
        //    {
        //        switch (value)
        //        {
        //            default:
        //                value += 11;
        //                break;
        //            case > 10:
        //                value += 1;
        //                break;
        //        }
        //    }

        //    return value;
        //}

        //private void CalculateDealerShowValue()
        //{
        //    dealerShowValue = 0;
        //    foreach (var card in GetDealerShow(dealerHand))
        //    {
        //        switch (card.CardValue)
        //        {
        //            default:
        //                dealerShowValue += card.CardValue;
        //                break;
        //            case 1:
        //                dealerShowValue += 11;
        //                break;
        //            case > 9:
        //                dealerShowValue += 10;
        //                break;
        //        }
        //    }
        //}
    }
}
