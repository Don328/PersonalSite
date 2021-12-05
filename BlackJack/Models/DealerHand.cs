using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public class DealerHand : Hand
    {
        private bool isActive;
        //private Card peekCard;

        public Card PeekCard()
        {
            if (Cards.Count < 1)
            {
                return new Card(0);
            }
            else
            {
                return Cards[0];
            }
        }

        public bool IsActive
        { get { return isActive; } }

        public int GetShowCardRankValue()
        {
            var firstCardValue = Cards[0].CardValue;

            switch (firstCardValue)
            {
                case 1:
                    return 11;
                case > 9:
                    return 10;
                default:
                    return firstCardValue;
            }


            return Cards[0].CardValue;
        }

        public bool CheckForHit()
        {
            isActive = true;

            if (IsBusted || IsBlackJack)
            {
                isActive = false;
            }

            if (Value > 17)
            {
                isActive = false;
            }

            if (Value == 17
                && !IsSoft)
            {
                isActive = false;
            }

            return isActive;
        }

        public void Show()
        {
            isActive = true;
        }
    }
}
