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
        private int baseValue = 0;
        protected int value = 0;
        protected bool isSoft = false;
        protected HandStatus status = HandStatus.New;
        protected string message = string.Empty;

        public Hand()
        {
            Cards = new List<Card>();
            status = HandStatus.New;
            baseValue = 0;
            value = 0;
            isSoft = false;
            message = string.Empty;
        }

        public List<Card> Cards
        { get; set; }

        public HandStatus Status
        { get { return status; } }

        public bool IsSoft
        { get { return isSoft; } }

        public int Value
        { get { return value; } }

        public string Message
        { get { return message; } }

        public abstract Hand Activate();
        
        public virtual async Task AddCard(Card card)
        {
            Cards.Add(card);
            baseValue = CalculateValue(Cards);
            await Task.CompletedTask;
        }

        protected virtual void CheckStatus()
        {
            switch (status)
            {
                case HandStatus.New:
                    CheckNewHandIsDelt();
                    break;
                case HandStatus.Dealt:
                    if (CheckIfBlackJack())
                    {
                        value = baseValue;
                        break;
                    }
                    CheckIsSoft();
                    break;
                case HandStatus.Active:
                    if (CheckIfBlackJack())
                    {
                        value = baseValue;
                        break;
                    }
                    CheckIsSoft();
                    CheckIfBusted();
                    break;
            }
        }

        public void CheckNewHandIsDelt()
        {
            if (Cards.Count == 2)
            {
                status = HandStatus.Dealt;
                CheckStatus();
            }
        }

        private int CalculateValue(List<Card> cards)
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

        protected void CheckIsSoft()
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
                if (baseValue > 21)
                {
                    isSoft = CheckCanUseBigAce(aces);
                    return;
                }

            }
            
            isSoft = hasAces;
            value = baseValue;
        }

        private bool CheckCanUseBigAce(List<Card> aces)
        {
            bool bigAces = false;
            var tempValue = baseValue;
            for (int i = aces.Count(); i > 0; i--)
            {
                tempValue -= 10;

                if (tempValue < 22)
                {
                    if (i > 1)
                    {
                        bigAces = true;
                    }
                    i = -1;
                }
            }

            value = tempValue;
            return bigAces;
        }

        protected bool CheckIfBusted()
        {
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
                && value == 21)
            {
                status = HandStatus.BlackJack;
                return true;
            }

            return false;
        }
    }
}
