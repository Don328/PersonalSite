using BlackJack.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public class PlayerHand : Hand
    {
        private bool isPaid = false;
        private int wager = 0;
        private HandResult result;

        public PlayerHand(int wager)
        {
            status = HandStatus.New;
            result = HandResult.Pending;
            isPaid = false;
            this.wager = wager;
        }

        public bool IsPaid
        { get { return isPaid; } }

        public int Wager
        { get { return wager; } }

        internal HandResult Result
        { get { return result; } }


        public override PlayerHand Activate()
        {
            status = HandStatus.Active;
            return this;
        }

        public override async Task AddCard(Card card)
        {
            await base.AddCard(card);
        }

        public override async Task<int> GetValue()
        {
            var aces = (
                from c in Cards
                where c.CardValue == 1
                select c).ToList();

            switch (baseValue)
            {
                case > 21:
                    if (!aces.Any())
                    {
                        status = HandStatus.Busted;
                        return await Task.FromResult(baseValue);
                    }
                    else
                    {
                        CheckIsSoft();
                        var adjusted = GetAdjustedValue(aces, baseValue);
                        if (adjusted > 21)
                        {
                            status = HandStatus.Busted;
                        }
                        return await Task.FromResult(adjusted);
                    }
                case < 21:
                    if (aces.Any())
                    {
                        CheckIsSoft();
                    }
                    return baseValue;

                case 21:
                    if (CheckIfBlackJack())
                    {
                        status = HandStatus.BlackJack;
                        return baseValue;
                    }
                    status = HandStatus.Pending;
                    return baseValue;
            }
        }

        public bool CanDouble() => Cards.Count == 2;
        public bool CanSplit() => IsPair(Cards);
        public bool IsBusted() => status == HandStatus.Busted;

        public async Task DoubleDown(Card card)
        {
            await base.AddCard(card);
            wager *= 2;
            Stay();

            await Task.CompletedTask;
        }

        public async Task<PlayerHand> Split()
        {
            var transferCard = Cards[1];

            var newHand =
                new PlayerHand(wager);

            await newHand.AddCard(transferCard);
            Cards.Remove(transferCard);

            return await Task.FromResult(newHand);
        }

        public void Stay()
        {
            status = HandStatus.Pending;
        }

        private bool IsPair(List<Card> cards)
        {
            if (CanDouble())
            {
                return cards[0].CardValue 
                    == cards[1].CardValue;
            }

            return false;
        }

        internal void SetResult(HandResult result)
        {
            this.result = result;
        }
    }

}
