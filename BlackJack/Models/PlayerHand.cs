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

        public PlayerHand(int wager)
        {
            status = HandStatus.New;
            isPaid = false;
            this.wager = wager;
        }

        public bool IsPaid
        { get { return isPaid; } }

        public int Wager
        { get { return wager; } }

        protected override void CheckStatus()
            => base.CheckStatus();

        public override PlayerHand Activate()
        {
            status = HandStatus.Active;
            return this;
        }

        public override async Task AddCard(Card card)
        {
            await base.AddCard(card);
            CheckStatus();
        }

        public bool CanDouble() => Cards.Count == 2;
        public bool CanSplit() => IsPair(Cards);
        public bool IsBusted() => status == HandStatus.Busted;

        public async Task DoubleDown(Card card)
        {
            await base.AddCard(card);
            CheckStatus();
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

        public void SetStatus(HandStatus status)
        {
            this.status = status;
        }
    }

}
