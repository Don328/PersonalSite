using BlackJack.Constants;
using BlackJack.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public class Player
    {
        private int bank = 0;
        private int wager = 10;
        private List<PlayerHand> hands
            = new List<PlayerHand>();
        private PlayerHand? activeHand;

        public Player(int bank)
        {
            hands = new List<PlayerHand>();
            activeHand = null;
            this.bank = bank;
        }

        public int Bank
        {
            get { return bank; }
            set { bank = value; }
        }
        public int Wager
        {
            get { return wager; }
            set { wager = value; }
        }

        public List<PlayerHand> Hands
        {
            get { return hands; }
        }

        public PlayerHand? ActiveHand
        { get { return activeHand; } }

        public async Task<PlayerHand> NewHand()
        {
            Hands.Clear();
            var hand = new PlayerHand(wager);
            Hands.Add(hand);
            activeHand = hand;
            bank -= wager;
            return await Task.FromResult(hand);
        }

        public bool HasEnoughBank() => bank >= wager;
        public bool HasActiveHand() => activeHand != null;
        public int ActiveWager() => activeHand?.Wager ?? 0;
        
        internal HandStatus? GetHandStatus() => activeHand?.Status;
        
        internal bool HasBlackJack() 
            => activeHand?.Cards.Count() == 2 
            && activeHand.Value.Result == 21;


        internal void SetHandStatus(HandStatus status)
            => activeHand?.SetStatus(status);

        internal void Stay()
        {
            activeHand?.SetStatus(HandStatus.Pending);
            activeHand = GetNext();
        }

        internal void Busted()
        {
            activeHand?.SetStatus(HandStatus.Busted);
            activeHand = GetNext();
        }

        internal async Task Action(
            PlayerOptions action,
            Card[]? cards = null)
        {
            if (cards is null)
            {
                // null card param results
                // in Stay action by default
                activeHand?.SetStatus(HandStatus.Pending);
                activeHand = GetNext();
            }
            else
            {
                switch (action)
                {

                    case PlayerOptions.Stay:
                        activeHand?.SetStatus(HandStatus.Pending);
                        activeHand = GetNext();
                        break;
                    case PlayerOptions.Hit:
                        ActiveHand?.AddCard(cards[0]);
                        break;
                    case PlayerOptions.DoubleDown:
                        activeHand?.DoubleDown(cards[0]);
                        break;
                    case PlayerOptions.Split:
                        await SplitHand(cards);
                        break;
                    default: break;
                }
            }
            

            await Task.CompletedTask;
        }

        internal void PayoutForBlackJack()
        {
            int bonus = 0;
            if (wager > 1)
            {
                bonus = wager / 2;
            }

            bank += wager * 2;
            bank += bonus;
            activeHand?.SetStatus(HandStatus.Paid);
            activeHand?.SetMessage(HandResultMessage.blackJack);
        }

        internal void PayoutForWin()
        {
            bank += wager * 2;
            activeHand?.SetStatus(HandStatus.Paid);
            activeHand?.SetMessage(HandResultMessage.playerWins);
        }

        internal void PayoutForPush()
        {
            bank += wager;
            activeHand?.SetStatus(HandStatus.Paid);
            activeHand?.SetMessage(HandResultMessage.push);
        }

        public PlayerHand? GetNext() => activeHand = (
                from h in Hands
                where h.Status == HandStatus.Dealt
                select h).FirstOrDefault()?.Activate();

        private async Task SplitHand(Card[] cards)
        {
            if (activeHand != null
                && HasEnoughBank())
            {
                var newHand = new PlayerHand(wager);
                var xCard = activeHand.Cards[1];
                await newHand.AddCard(xCard);
                activeHand.Cards.Remove(xCard);
                await activeHand.AddCard(cards[0]);
                await newHand.AddCard(cards[1]);
                hands.Add(newHand);
            }
        }
    }
}
