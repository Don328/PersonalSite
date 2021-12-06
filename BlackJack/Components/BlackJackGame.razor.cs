using BlackJack.Constants;
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
        private Deck deck = new();
        private Queue<Card> drawPile = new();
        private DealerHand dealerHand = new();
        private Player player = new(200);
        private List<PlayerOptions> playerActions = new();
        private bool showPlayerActions = false;
        private PlayerHand activeHand = new(0);


        // Distribut dealer message to individual hands?
        // Or Separate responsiblities and inclued global dealer message
        private string dealerMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            player = new Player(200);
            player.Wager = 10;

            await Task.CompletedTask;
        }

        private async Task NewHand()
        {
            var validator = WagerValidator.IsValid(player.Wager);

            if (player.HasEnoughBank()
                && validator
                == WagerValidator.successMessage)
            {
                await SetupHand();
                await DealCards();

                await Task.Delay(500);



                if (HandIsBlackJack(dealerHand))
                {
                    await EndHand();
                    return;
                }

                await WaitForActions();
            }
        }

        private async Task SetupHand()
        {
            SetupDeck();
            SetupDealer();
            await SetupPlayer();
        }

        private void SetupDeck()
        {
            deck.Cards.Clear();
            deck = new Deck(DeckType.Shuffled);

            drawPile.Clear();
            drawPile = new Queue<Card>(deck.NoJokers());

            dealerMessage = "Dealing Cards";
        }

        private void SetupDealer()
        {
            dealerHand.Cards.Clear();
            dealerHand = new();
            showDealButton = false;
        }

        private async Task SetupPlayer()
        {
            await Task.Delay(500);
            activeHand = await player.NewHand();
            dealerMessage = String.Empty;
        }


        private async Task DealCards()
        {
            var burn = drawPile.Dequeue();
            await activeHand.AddCard(drawPile.Dequeue());
            StateHasChanged();
            await Task.Delay(500);

            await dealerHand.AddCard(drawPile.Dequeue());
            StateHasChanged();
            await Task.Delay(500);

            await activeHand.AddCard(drawPile.Dequeue());
            StateHasChanged();
            await Task.Delay(500);

            await dealerHand.AddCard(drawPile.Dequeue());
            StateHasChanged();
            await Task.Delay(500);
        }

        private bool HandIsBlackJack(Hand hand) =>
            hand.Status == HandStatus.BlackJack;

        private bool HandIsBusted(Hand hand) =>
            hand.Status == HandStatus.Busted;

        private async Task WaitForActions()
        {
            await CheckPlayerBlackJack();

            switch (activeHand.Status)
            {
                case HandStatus.Busted:
                    await GetNextHand();
                    break;
                case HandStatus.Pending:
                    await GetNextHand();
                    break;
                case HandStatus.Paid:
                    await GetNextHand();
                    break;
                case HandStatus.Dealt:
                    activeHand.SetStatus(HandStatus.Active);
                    AddPlayerOptions();
                    break;
                case HandStatus.Active:
                    AddPlayerOptions();
                    break;
            }

            await Task.CompletedTask;
        }

        private async Task GetNextHand()
        {
            var next = player.GetNext();

            if (next == null)
            {
                if ((from h in player.Hands
                    where h.Status == HandStatus.Pending
                    select h).Any())
                {
                    await PlayDealerHand();
                }
                else
                {
                    await EndPlayerHand();
                }
            }
            else
            {
                activeHand = next;
                await WaitForActions();
            }
            
            await Task.CompletedTask;
        }

        private void AddPlayerOptions()
        {
            dealerMessage = "Player's Move";

            playerActions.Clear();
            playerActions.Add(PlayerOptions.Hit);
            playerActions.Add(PlayerOptions.Stay);
            if (activeHand.CanDouble())
            {
                playerActions.Add(PlayerOptions.DoubleDown);

                if (activeHand.CanSplit())
                {
                    playerActions.Add(PlayerOptions.Split);
                }
            }

            showPlayerActions = true;
            StateHasChanged();
        }

        private async Task PlayerStays()
        {
            showPlayerActions = false;
            activeHand.SetStatus(HandStatus.Pending);
            StateHasChanged();
            await GetNextHand();
        }

        private async void PlayerHits()
        {
            showPlayerActions = false;
            await Task.Delay(500);
            await activeHand.AddCard(drawPile.Dequeue());
            StateHasChanged();
            await WaitForActions();
        }

        private async Task PlayerDoubles()
        {
            dealerMessage = "Double Down";
            showPlayerActions = false;
            await Task.Delay(500);
            await activeHand.DoubleDown(drawPile.Dequeue());
            StateHasChanged();
            await WaitForActions();
        }

        private async Task PlayerSplits()
        {
            showPlayerActions = false;
            await Task.Delay(500);
            if (player.HasEnoughBank() &&
                activeHand.CanSplit())
            {
                dealerMessage = "Split";
                var newHand = new PlayerHand(player.Wager);
                var transferCard = activeHand.Cards[1];
                newHand.Cards.Add(transferCard);
                activeHand.Cards.Remove(transferCard);
                await activeHand.AddCard(drawPile.Dequeue());
                await newHand.AddCard(drawPile.Dequeue());
                player.Hands.Add(newHand);
            }

            StateHasChanged();
            await WaitForActions();
        }

        private async Task PlayDealerHand()
        {
            dealerMessage = String.Empty;
            dealerHand.Activate();
            dealerMessage = "Dealer's Move";
            StateHasChanged();

            dealerHand.CheckForHold();

            if (dealerHand.Status == HandStatus.Active)
            {
                await DealerHits();
            }

            await EndHand();
        }

        private async Task DealerHits()
        {
            await Task.Delay(500);
            await dealerHand.AddCard(drawPile.Dequeue());
            StateHasChanged();
            await PlayDealerHand();
        }

        private async Task EndPlayerHand()
        {
            if ((from h in player.Hands
                 where h.Status == HandStatus.Pending
                 select h).Any())
            {
                await PlayDealerHand();
            }
            else
            {
                await EndHand();
            }

        }

        private async Task EndHand()
        {
            showPlayerActions = false;

            switch (dealerHand.Status)
            {
                case HandStatus.BlackJack:
                    foreach (var h in player.Hands)
                    {
                        if (h.Status == HandStatus.BlackJack)
                            h.SetStatus(HandStatus.Push);
                        else h.SetStatus(HandStatus.Lose);
                    }
                    break;
                case HandStatus.Busted:
                    foreach (var h in player.Hands)
                    {
                        if (h.Status == HandStatus.Pending)
                        {
                            h.SetStatus(HandStatus.Win);
                        }
                    }
                    break;
                case HandStatus.Hold:
                    foreach (var h in player.Hands)
                    {
                        if (h.Status == HandStatus.Pending)
                        {
                            if (h.Value > dealerHand.Value)
                                h.SetStatus(HandStatus.Win);
                            if (h.Value == dealerHand.Value)
                                h.SetStatus(HandStatus.Push);
                            if (h.Value < dealerHand.Value)
                                h.SetStatus(HandStatus.Lose);
                        }
                    }
                    break;
            }

            await PayoutWins();
            await Task.CompletedTask;
        }

        private async Task CheckPlayerBlackJack()
        {
            if (activeHand.Status == HandStatus.BlackJack)
            {
                var bonus = activeHand.Wager / 2;
                player.Bank += activeHand.Wager * 2;
                player.Bank += bonus;
                activeHand.SetStatus(HandStatus.Paid);
            }

            await Task.CompletedTask;
        }

        private async Task PayoutWins()
        {
            foreach(var hand in player.Hands)
            {
                switch (hand.Status)
                {
                    case HandStatus.Win:
                        player.Bank += hand.Wager * 2;
                        break;
                    case HandStatus.Push:
                        player.Bank += hand.Wager;
                        break;
                }
            }

            showDealButton = true;
            await Task.CompletedTask;
        }
    }
}
