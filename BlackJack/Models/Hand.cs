using BlackJack.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public abstract class Hand
    {
        protected List<Card> cards;
        protected int baseValue = 0;
        protected bool isSoft = false;
        protected HandStatus status = HandStatus.New;
        protected string message = string.Empty;

        public Hand()
        {
            cards = new List<Card>();
            status = HandStatus.New;
            baseValue = 0;
            isSoft = false;
            message = string.Empty;
        }

        public List<Card> Cards
        { get { return cards; } }

        public HandStatus Status
        { get { return status; } }

        public bool IsSoft
        { get { return isSoft; } }

        public Task<int> Value
        { get { return GetValue(); } }

        public string Message
        { get { return message; } }

        public abstract Hand Activate();
        public abstract Task<int> GetValue();

        internal virtual void Clear()
        {
            cards.Clear();
            status = HandStatus.New;
            isSoft = false;
            message = string.Empty;
        }

        public virtual async Task AddCard(Card card)
        {
            Cards.Add(card);

            if (Cards.Count == 2) status = HandStatus.Dealt;
            if (cards.Count > 2) status = HandStatus.Active;

            baseValue = CalculateBaseValue(Cards);
            await GetValue();
            await Task.CompletedTask;
        }


        protected int GetAdjustedValue(List<Card> aces, int val)
        {
            int adjusted = val;
            adjusted -= 10;
            var ace = (from c in aces
                       select c).First();
            aces.Remove(ace);

            isSoft = aces.Any();

            if (adjusted > 21
            && isSoft)
            {
                GetAdjustedValue(aces, adjusted);
            }

            return adjusted;
        }

        private int CalculateBaseValue(List<Card> cards)
        {
            int newValue = 0;
            foreach (Card card in cards)
            {
                switch (card.CardValue)
                {
                    case > 9:
                        newValue += 10;
                        break;
                    case 1:
                        newValue += 11;
                        break;
                    default:
                        newValue += card.CardValue;
                        break;
                }
            }

            return newValue;
        }

        protected bool CheckIsSoft()
        {
            isSoft = false;

            var aces = (from c in Cards
                        where c.CardValue == 1
                        select c).ToList();
            var noAces = (from c in Cards
                          where !aces.Any()
                          select c).ToList();

            var hasAces = aces.Any();

            if (hasAces)
            {
                if (baseValue > 21
                    && GetAdjustedValue(aces, baseValue) < 22)
                {
                    return isSoft;
                }

            }

            isSoft = hasAces;
            return isSoft;
        }

        protected async Task<bool> CheckIfBusted()
        {
            var value = await Value;

            if (!isSoft &&
                value > 21)
            {
                status = HandStatus.Busted;
                return true;
            }

            return false;
        }

        protected bool CheckIfBlackJack()
        {
            if (Cards.Count() == 2
                && baseValue == 21)
            {
                return true;
            }

            return false;
        }

        public void SetMessage(string message)
        {
            this.message = message;
        }

        public void SetStatus(HandStatus status)
        {
            this.status = status;
        }
    }
}
