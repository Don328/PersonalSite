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
        private int wager = 0;
        private List<PlayerHand> hands
            = new List<PlayerHand>();


        public Player(int bank)
        {
            hands = new List<PlayerHand>();
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

        public async Task<PlayerHand> NewHand()
        {
            Hands.Clear();
            var hand = new PlayerHand(wager);
            Hands.Add(hand);
            bank -= wager;
            return await Task.FromResult(hand);
        }

        public bool HasEnoughBank() => bank >= wager;

        public PlayerHand? GetActiveHand() => (
                from h in Hands
                where h.Status == HandStatus.Active
                select h).FirstOrDefault();

        public PlayerHand? GetNext() => (
                from h in Hands
                where h.Status == HandStatus.Dealt
                select h).FirstOrDefault()?.Activate();
    }
}
