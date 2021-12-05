using BlackJack.Enums;
using BlackJack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Services
{
    internal static class Dealer
    {
        internal static List<Card> GetSortedDeck()
        {
            var deck = new List<Card>();

            for (int i = 0; i < 54; i++)
            {
                deck.Add(new Card((CardValueIndex)i));
            }

            return deck;
        }

        internal static List<Card> GetShuffledDeck()
        {
            int deckSize = 54;
            var deck = new List<Card>();
            var table = new bool[deckSize];

            for (int i = 0; i < deckSize; i++)
            {
                table[i] = false;
            }

            while (deck.Count < deckSize)
            {
                var random = new Random();
                int value = random.Next(54);
                if (!table[value])
                {
                    deck.Add(new Card((CardValueIndex)value));
                    table[value] = true;
                }

                continue;
            }

            return deck;
        }
    }
}
