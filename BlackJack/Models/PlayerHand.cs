using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public class PlayerHand : Hand
    {
        private int wager = 0;
        private bool isFirstMove = true;
        private bool isPair = false;
        private bool stay = false;

        public PlayerHand(int wager)
        {
            this.wager = wager;
            stay = false;
        } 

        public int Wager 
        { get { return wager; } }

        public bool IsFirstMove
        { get { return isFirstMove; } }

        public bool IsPair
        { get { return isPair; } }

        public bool Stayed
        { 
            get { return stay; }
        }

        public override async Task AddCard(Card card)
        {
            await base.AddCard(card);
            CheckIsFirstMove(Cards);
            if (isFirstMove)
            {
                CheckIsPair(Cards);
            }
        }

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
            stay = true;
        }

        private bool CheckIsFirstMove(List<Card> cards)
        {
            isFirstMove = false;

            if (cards.Count == 2)
                isFirstMove = true;
            
            return isFirstMove;
        }


    private bool CheckIsPair(List<Card> cards)
        {
            isPair = false;
            if (CheckIsFirstMove(cards))
            {
                if (cards[0].CardValue 
                    == cards[1].CardValue)
                {
                    isPair = true;
                }
            }
            
            return isPair;
        }
    }
}
