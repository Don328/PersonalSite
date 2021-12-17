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
        }

        public override async Task<int> GetValue()
        {
            var aces = (
            from c in Cards
            where c.CardValue == 1
            select c).ToList();

            switch (baseValue)
            {
                case 21:
                    if (CheckIfBlackJack())
                    {
                        status = HandStatus.BlackJack;
                        return baseValue;
                    }
                    status = HandStatus.Hold;
                    return baseValue;
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
                        else if (adjusted > 16)
                        {
                            status = HandStatus.Hold;
                        }

                        return adjusted;
                    }
                case < 21:
                    if (aces.Any())
                    {
                        CheckIsSoft();
                    }

                    if (baseValue > 17)
                    {
                        status = HandStatus.Hold;
                    }
                    if (baseValue == 17)
                    {
                        if (!isSoft)
                        {
                            status = HandStatus.Hold;
                        }
                    }

                    return baseValue;
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

        //public async void CheckForHold()
        //{
        //    var value = await Value;
        //    if (status == HandStatus.Active)
        //    {
        //        if (value > 17)
        //        {
        //            status = HandStatus.Hold;
        //        }

        //        if (value == 17
        //            && !IsSoft)
        //        {
        //            status = HandStatus.Hold;
        //        }
        //    }
        //}
    }
}
