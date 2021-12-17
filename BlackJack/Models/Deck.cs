using BlackJack.Constants;
using BlackJack.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public class Deck : IDisposable
    {
        private readonly DeckType deckType;
        private readonly List<Card> cards;

        /// <summary>
        ///  Creates a new deck. Passing in a DeckType will crate a deck of a specific type
        ///  An empty constructor creates a "Standard" deck
        /// </summary>
        /// <param name="deckType">
        /// - Shuffled: Has jokers
        /// - Standard: Shuffled w/o jokers
        /// - Packaged: Sorted with jokers
        /// - Sorted: No jokers
        /// </param>

        public Deck(
            DeckType deckType = DeckType.Standard)
        {
            this.deckType = deckType;

            switch(deckType)
            {
                case DeckType.Standard:
                    cards = GetDeck(DeckType.Shuffled);
                    cards = this.NoJokers();
                    break;
                case DeckType.Sorted:
                    cards= GetDeck(DeckType.Sorted);
                    cards = this.NoJokers();
                    break;
                case DeckType.Shuffled:
                    cards = GetDeck(DeckType.Shuffled);
                    break;
                case DeckType.Packaged:
                    cards = GetDeck(DeckType.Sorted);
                    break;
                default:
                    break;
            }
        }

        public List<Card> Cards
        { get { return cards; } }

        private List<Card> GetDeck(DeckType deckType)
        {
            switch (deckType)
            {
                default:
                    return new List<Card>();
                case DeckType.Sorted:
                    return GetSortedDeck();
                case DeckType.Shuffled:
                    return GetShuffledDeck();
            }
        }

        public List<Card> NoJokers()
        {
            return (from c in cards
                    where c.Suit != CardSuit.joker
                    select c).ToList();
        }

        public List<Card> OneSuit(string suit)
        {
            return (from c in cards
                    where c.Suit == suit
                    select c).ToList();
        }

        public List<Card> JustTen(
            string suit = CardSuit.spades)
        {
            int aceIndex;
            int lowerBounds;
            int upperBounds;

            switch (suit)
            {
                default:
                    return new List<Card>();
                case CardSuit.hearts:
                    aceIndex = 13;
                    lowerBounds = 0;
                    upperBounds = 10;
                    break;
                case CardSuit.spades:
                    aceIndex = 26;
                    lowerBounds = 13;
                    upperBounds = 23;
                    break;
                case CardSuit.diamonds:
                    aceIndex = 39;
                    lowerBounds = 26;
                    upperBounds = 36;
                    break;
                case CardSuit.clubs:
                    aceIndex = 52;
                    lowerBounds = 39;
                    upperBounds = 49;
                    break;
            }

            var deck = new List<Card>();

            deck.Add((from c in cards
                      where (int)c.Index == aceIndex
                      select c).First());

            var numCards = (from c in cards
                            where (int)c.Index > lowerBounds
                            && (int)c.Index < upperBounds
                            select c).ToList();

            foreach (var c in numCards)
            {
                deck.Add(c);
            }
            return deck;

        }

        private List<Card> GetSortedDeck()
        {
            var deck = new List<Card>();

            for (int i = 0; i < 54; i++)
            {
                deck.Add(new Card((CardValueIndex)i));
            }

            return deck;
        }

        private List<Card> GetShuffledDeck()
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

        public void Dispose()
        {
            cards.Clear();
        }
    }
}
