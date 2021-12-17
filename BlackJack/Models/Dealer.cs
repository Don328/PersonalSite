using BlackJack.Enums;

namespace BlackJack.Models
{
    internal class Dealer
    {
        private Deck deck;
        private Queue<Card> drawPile;
        private DealerHand hand;

        public Dealer()
        {
            deck = new();
            drawPile = new Queue<Card>();
            hand = new();
            Shuffle();
        }

        public Dealer(DeckType type)
        {
            deck = new(type);
            drawPile = new();
            hand = new();
        }

        public DealerHand Hand
        { get { return hand; } }

        public Deck Deck
        { get { return deck; } }

        public void Shuffle()
        {
            hand.Clear();
            deck.Dispose();
            deck = new();
            drawPile.Clear();
            drawPile = new Queue<Card>(deck.Cards);
        }

        public void BurnCard()
        {
            drawPile.Dequeue();
        }

        public Card DrawCard()
        {
            return drawPile.Dequeue();
        }

        public async Task Hit()
        {
            await hand.AddCard(DrawCard());
        }

        public async Task<bool> HasBlackJack()
        {
            var value = await hand.Value;

            if (hand.Cards.Count == 2 
            && value == 21)
            {
                hand.SetStatus(HandStatus.BlackJack);
                return true;
            }

            return false;
        }
    }
}
