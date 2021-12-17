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
        private bool hideDealerHand = true;
        private Dealer dealer = new();
        private Player player = new(200);
        private List<PlayerOptions> playerActions = new();
        private bool showPlayerActions = false;



        private string dealerMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            showDealButton = true;
            await base.OnInitializedAsync();
        }

        private async Task NewHand()
        {
            dealerMessage = String.Empty;
            hideDealerHand = true;

            var validator = new WagerValidator(player.Wager);

            dealerMessage = validator.Result;
            
            if (validator.Result == validator.Success)
            {
                if (player.HasEnoughBank())
                {
                    showDealButton = false;
                    await SetupHand();
                    await DealCards();

                    await Task.Delay(500);

                    if (await dealer.HasBlackJack())
                    {
                        await EndHand();
                        return;
                    }

                    await WaitForActions();
                }
                else
                {
                    dealerMessage = "You don't have that much to wager";
                }
            }
        }

        private async Task SetupHand()
        {
            dealer.Shuffle();
            dealerMessage = "Dealing Cards";
            await Task.Delay(500);
            await player.NewHand();
            dealerMessage = String.Empty;
        }

        private async Task DealCards()
        {
            dealer.BurnCard();
            await player.Action(
                PlayerOptions.Hit,
                new Card[]
                {
                    dealer.DrawCard()
                });
            StateHasChanged();
            await Task.Delay(500);

            await dealer.Hit();
            StateHasChanged();
            await Task.Delay(500);

            await player.Action(
                PlayerOptions.Hit,
                new Card[]
                {
                    dealer.DrawCard()
                });
            StateHasChanged();
            await Task.Delay(500);

            await dealer.Hit();
            StateHasChanged();
            await Task.Delay(500);
        }

        private async Task WaitForActions()
        {
            if (player.ActiveHand == null
                && player.GetNext() == null)
            {
                showDealButton = true;
                if (CheckForPendingHands())
                {
                    await DrawDealerHand();
                }
                else
                {
                    await EndHand();
                }

                return;
            }

            await CheckPlayerBlackJack();

            switch (player.GetHandStatus())
            {
                case HandStatus.Busted:
                    player.Busted();
                    await WaitForActions();
                    break;
                case HandStatus.Pending:
                    await player.Action(PlayerOptions.Stay);
                    player.GetNext();
                    await WaitForActions();
                    break;
                case HandStatus.BlackJack:
                    player.GetNext();
                    await WaitForActions();
                    break;
                case HandStatus.Dealt:
                    AddPlayerOptions();
                    player.SetHandStatus(HandStatus.Active);
                    await WaitForActions();
                    break;
                case HandStatus.Active:
                    AddPlayerOptions();
                    break;
                case null:
                    if (CheckForPendingHands())
                    {
                        dealer.Hand.Activate();
                        await DrawDealerHand();
                    };

                    await EndHand();
                    break;
                default:
                    break;
            }
        }

        private void AddPlayerOptions()
        {
            dealerMessage = "Player's Move";

            playerActions.Clear();
            playerActions.Add(PlayerOptions.Hit);
            playerActions.Add(PlayerOptions.Stay);
            if (player.ActiveHand != null &&
                player.ActiveHand.CanDouble())
            {
                playerActions.Add(PlayerOptions.DoubleDown);

                if (player.ActiveHand.CanSplit())
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
            await player.Action(PlayerOptions.Stay);
            StateHasChanged();
            await WaitForActions();
        }

        private async void PlayerHits()
        {
            if (player.ActiveHand != null)
            {

                showPlayerActions = false;
                await Task.Delay(500);
                
                await player.Action(
                    PlayerOptions.Hit,
                    new Card[] 
                    { 
                        dealer.DrawCard()
                    });
                StateHasChanged();
                await WaitForActions();
            }
        }

        private async Task PlayerDoubles()
        {
            if (player.ActiveHand != null)
            {
                dealerMessage = "Double Down";
                showPlayerActions = false;
                await Task.Delay(500);
                await player.Action(
                    PlayerOptions.DoubleDown,
                    new Card[]
                    { 
                        dealer.DrawCard() 
                    });
                StateHasChanged();
                await WaitForActions();
            }

        }

        private async Task PlayerSplits()
        {
            if (player.ActiveHand != null)
            {
                showPlayerActions = false;
                await Task.Delay(500);
                if (player.HasEnoughBank() &&
                    player.ActiveHand.CanSplit())
                {
                    Card[] newCards = new Card[2]
                    {
                        dealer.DrawCard(),
                        dealer.DrawCard()
                    };

                    dealerMessage = "Split";
                    await player.Action(
                        PlayerOptions.Split,
                        newCards);
                
                    StateHasChanged();
                }

            }

            await WaitForActions();
        }

        private bool CheckForPendingHands()
        {
            return ((from h in player.Hands
                     where h.Status == HandStatus.Pending
                     select h).Any());
        }

        private async Task DrawDealerHand()
        {
            hideDealerHand = false;
            dealerMessage = String.Empty;
            dealerMessage = "Dealer's Move";
            StateHasChanged();

            if (dealer.Hand.Status == HandStatus.Active
                || dealer.Hand.Status == HandStatus.Dealt)
            {
                await DealerHits();
            }
            else
            {
                await EndHand();
            }

        }

        private async Task DealerHits()
        {
            dealer.Hand.SetStatus(HandStatus.Active);
            await Task.Delay(1000);
            await dealer.Hit();
            StateHasChanged();
            await DrawDealerHand();
        }

        private async Task EndHand()
        {
            dealerMessage = string.Empty;
            hideDealerHand = false;
            StateHasChanged();
            switch (dealer.Hand.Status)
            {
                case HandStatus.BlackJack:
                    dealer.Hand.SetMessage(HandResultMessage.dealerBlackJack);
                    foreach (var h in player.Hands)
                    {
                        if (h.Status == HandStatus.BlackJack)
                        {
                            h.SetResult(HandResult.Push);
                        }
                        else
                        {
                            h.SetResult(HandResult.Loss);
                        }
                    }
                    await Payout();
                    break;
                case HandStatus.Busted:
                    dealer.Hand.SetMessage(HandResultMessage.busted);
                    foreach (var h in player.Hands)
                    {
                        if (h.Status == HandStatus.Pending)
                        {
                            h.SetResult(HandResult.Win);
                        }
                    }
                    await Payout();
                    break;
                case HandStatus.Hold:
                    var dealerValue = await dealer.Hand.Value;
                    foreach (var h in player.Hands)
                    {
                        var handValue = await h.Value;
                        if (h.Status == HandStatus.Pending)
                        {
                            if (handValue > dealerValue)
                                h.SetResult(HandResult.Win);
                            if (handValue == dealerValue)
                                h.SetResult(HandResult.Push);
                            if (handValue < dealerValue)
                            {
                                h.SetResult(HandResult.Loss);
                                h.SetMessage(HandResultMessage.dealerWins);
                            }
                        }
                    }

                    await Payout();
                    break;
                default:
                    break;
            }

            showDealButton = true;
            await Task.CompletedTask;
        }

        private async Task Payout()
        {
            foreach (var h in player.Hands)
            {
                switch (h.Result)
                {
                    case HandResult.Win:
                        player.PayoutForWin();
                        break;
                    case HandResult.Push:
                        player.PayoutForPush();
                        break;
                    case HandResult.BlackJack:
                        if (h.Status != HandStatus.Paid)
                        {
                            player.PayoutForBlackJack();
                        }
                        break;
                    case HandResult.Bust:
                        break;
                    case HandResult.Loss:
                        break;
                    default:
                        break;
                }
            }

            await Task.CompletedTask;
        }

        private async Task CheckPlayerBlackJack()
        {
            if (player.HasBlackJack())
            {
                player.ActiveHand?
                    .SetResult(HandResult.BlackJack);
                player.PayoutForBlackJack();
                player.GetNext();
                await WaitForActions();
            }

            await Task.CompletedTask;
        }
    }
}
