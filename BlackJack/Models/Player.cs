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
        private int wager = 0;

        public List<PlayerHand> Hands { get; set; }

        public Player(int bank)
        {
            Hands = new List<PlayerHand>();
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

        public bool IsActive
        { get { return HasActiveHands(); } }

        public bool HasEnoughBank
        { get { return HasBank(); } }
        
        public async Task<List<Card>> SplitHand(List<Card> hand)
        {
            var transferCard = hand[1];
            var newHand = new PlayerHand(wager);
            await newHand.AddCard(transferCard);
            hand.Remove(transferCard);
            Hands.Add(newHand);
            return await Task.FromResult(hand);
        }

        private bool HasBank()
        {
            return bank > wager;
        }

        private bool HasActiveHands()
        {
            return (from h in Hands
                where h.Stayed == false
                && h.IsBusted == false
                select h).Any();
        }
    }
}
