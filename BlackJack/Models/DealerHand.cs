using BlackJack.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public class DealerHand : Hand
    {
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

        public override Hand Activate()
        {
            status = HandStatus.Active;
            return this;
        }

        public override async Task AddCard(Card card)
        {
            await base.AddCard(card);
            CheckStatus();
        }

        protected override void CheckStatus()
        {
            switch (status)
            {
                case HandStatus.New:
                    base.CheckStatus();
                    break;
                case HandStatus.Dealt:
                    base.CheckStatus();
                    break;
                case HandStatus.Active:
                    CheckIsSoft();
                    if (CheckIfBusted())
                    {
                        break;
                    }
                    
                    CheckForHold();
                    break;
            }
        }

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
        }

        public void CheckForHold()
        {
            if (status == HandStatus.Active)
            {
                if (Value > 17)
                {
                    status = HandStatus.Hold;
                }

                if (Value == 17
                    && !IsSoft)
                {
                    status = HandStatus.Hold;
                }
            }
        }
    }
}
