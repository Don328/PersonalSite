using AlgoCards.Constants;
using AlgoCards.Enums;
using AlgoCards.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoCards.Models
{
    public class Deck
    {
        private readonly DeckType deckType;
        private readonly List<Card> cards;

        public List<Card> Cards
        { get { return cards; } }

        public Deck(
            DeckType deckType = DeckType.Shuffled)
        {
            this.deckType = deckType;
            cards = GetDeck(deckType);
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

        private List<Card> GetDeck(DeckType deckType)
        {
            switch (deckType)
            {
                default:
                    return new List<Card>();
                case DeckType.Sorted:
                    return Dealer.GetSortedDeck();
                case DeckType.Shuffled:
                    return Dealer.GetShuffledDeck();
            }
        }
    }
}
