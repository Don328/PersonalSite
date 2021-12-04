using AlgoCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public abstract class Hand
    {
        private int value = 0;
        private bool isBusted = false;
        private bool isSoft = false;
        private bool isBlackJack = false;
        private string dealerMessage = string.Empty;

        public Hand()
        {
            Cards = new List<Card>();
        }

        public List<Card> Cards
        { get; set; }
        
        public bool IsBusted 
        { get { return isBusted; } }
        
        public bool IsSoft
        { get { return isSoft; } }
        
        public int Value
        { get { return value; } }

        public bool IsBlackJack
        { get { return isBlackJack; } }

        public string DealerMessage
        { 
            get { return dealerMessage; }
            set { dealerMessage = value; }
        }

        public virtual async Task AddCard(Card card)
        {
            Cards.Add(card);
            value = CalculateHandValue();
            isBusted = CheckIfBusted();
            isBlackJack = CheckIfBlackJack();

            await Task.CompletedTask;
        }

        private int CalculateHandValue()
        {
            int newValue = 0;
            List<Card> aces = new();
            
            foreach (Card card in Cards)
            {
                switch (card.CardValue)
                {
                    case > 9:
                        newValue += 10;
                        break;
                    case 1:
                        aces.Add(card);
                        break;
                    default:
                        newValue += card.CardValue;
                        break;
                }
            }

            if (aces.Count > 0)
            {
                int acesValue = 0;
                foreach (var ace in aces)
                {
                    switch (newValue)
                    {
                        case > 10:
                            acesValue += 1;
                            newValue += 1;
                            break;
                        default:
                            acesValue += 11;
                            newValue += 11;
                            break;
                    }
                }

                isSoft = CheckIsSoft(
                    aces.Count, acesValue);
            }


            return newValue;
        }

        private bool CheckIsSoft(int acesCount, int acesValue)
        {
            if (acesCount < acesValue)
                return true;
            return false;
        }

        private bool CheckIfBusted()
        {
            if (value > 21)
            {
                isBusted = true;
            }

            return isBusted;
        }

        private bool CheckIfBlackJack()
        {
            isBlackJack = false;

            if (Cards.Count == 2
                && value == 21)
            {
                isBlackJack = true;
                isSoft = false;
            }

            return isBlackJack;
        }

        public void SetDealerMessage(string message)
        {
            dealerMessage = message;
        }
    }
}
