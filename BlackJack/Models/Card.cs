using BlackJack.Constants;
using BlackJack.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public class Card
    {
        private readonly string suit;
        private readonly string suitColor;
        private readonly int cardValue;
        private readonly string rank;
        private readonly CardValueIndex index;

        public Card(string suit, string rank)
        {
            this.suit = suit;
            this.suitColor = GetSuitColor();
            this.rank = rank;
            index = GetIndex();
        }

        public Card(CardValueIndex index)
        {
            this.index = index;
            char r = GetRankValue();
            char s = GetSuitValue();
            this.rank = GetRankName(r);
            this.suit = GetSuitName(s);
            this.suitColor = GetSuitColor();
            this.cardValue = GetValue();
        }

        public CardValueIndex Index
        { get { return index; } }
        public string Suit
        { get { return suit; } }
        public string Rank
        { get { return rank; } }
        public string SuitColor
        { get { return suitColor; } }
        public int CardValue
        { get { return cardValue; } }

        public override string ToString()
        {
            if (rank.CompareTo("joker") == 0)
            {
                return "JOKER";
            }

            var valueString = cardValue.ToString();
            if (cardValue == 1 ||
                cardValue > 10)
            {
                valueString = index.ToString()[1].ToString();
            }

            return valueString + " of " + suit;
        }

        private string GetSuitColor()
        {
            switch (suit)
            {
                default:
                    return "#00FFFF";
                case CardSuit.hearts:
                    return CardColor.hearts;
                case CardSuit.spades:
                    return CardColor.spades;
                case CardSuit.diamonds:
                    return CardColor.diamonds;
                case CardSuit.clubs:
                    return CardColor.spades;
                case CardSuit.joker:
                    return CardColor.joker;
            }
        }

        private CardValueIndex GetIndex()
        {
            StringBuilder builder = new();
            builder.Append(suit.ToUpper()[0]);
            char rankValue;

            switch (rank)
            {
                case CardRank.two:
                    rankValue = '2';
                    break;
                case CardRank.three:
                    rankValue = '3';
                    break;
                case CardRank.four:
                    rankValue = '4';
                    break;
                case CardRank.five:
                    rankValue = '5';
                    break;
                case CardRank.six:
                    rankValue = '6';
                    break;
                case CardRank.seven:
                    rankValue = '7';
                    break;
                case CardRank.eight:
                    rankValue = '8';
                    break;
                case CardRank.nine:
                    rankValue = '9';
                    break;
                case CardRank.ten:
                    rankValue = 'T';
                    break;
                case CardRank.jack:
                    rankValue = 'J';
                    break;
                case CardRank.queen:
                    rankValue = 'Q';
                    break;
                case CardRank.king:
                    rankValue = 'K';
                    break;
                case CardRank.ace:
                    rankValue = 'A';
                    break;
                default:
                    rankValue = 'X';
                    return Joker();
            }

            builder.Append(rankValue);

            for (int i = 0; i < 52; i++)
            {
                var idx = (CardValueIndex)i;
                if (idx.ToString() == builder.ToString())
                {
                    return idx;
                }
            }

            // return joker 'XX,0' if card value is not found

            return (CardValueIndex)0;
        }

        private CardValueIndex Joker()
        {
            if (suit == "J")
            {
                return (CardValueIndex)53;
            }

            return (CardValueIndex)0;
        }

        private char GetRankValue()
        {
            return index.ToString()[1];
        }

        private string GetRankName(char r)
        {
            switch (r)
            {
                default:
                    return CardRank.joker;
                case '2':
                    return CardRank.two;
                case '3':
                    return CardRank.three;
                case '4':
                    return CardRank.four;
                case '5':
                    return CardRank.five;
                case '6':
                    return CardRank.six;
                case '7':
                    return CardRank.seven;
                case '8':
                    return CardRank.eight;
                case '9':
                    return CardRank.nine;
                case 'T':
                    return CardRank.ten;
                case 'J':
                    return CardRank.jack;
                case 'Q':
                    return CardRank.queen;
                case 'K':
                    return CardRank.king;
                case 'A':
                    return CardRank.ace;

            }
        }

        private char GetSuitValue()
        {
            return index.ToString()[0];
        }

        private string GetSuitName(char s)
        {
            switch (s)
            {
                default:
                    return CardSuit.joker;
                case 'H':
                    return CardSuit.hearts;
                case 'S':
                    return CardSuit.spades;
                case 'D':
                    return CardSuit.diamonds;
                case 'C':
                    return CardSuit.clubs;
            }
        }

        private int GetValue()
        {
            switch (rank)
            {
                default:
                    return -1;
                case CardRank.joker:
                    return 0;
                case CardRank.ace:
                    return 1;
                case CardRank.two:
                    return 2;
                case CardRank.three:
                    return 3;
                case CardRank.four:
                    return 4;
                case CardRank.five:
                    return 5;
                case CardRank.six:
                    return 6;
                case CardRank.seven:
                    return 7;
                case CardRank.eight:
                    return 8;
                case CardRank.nine:
                    return 9;
                case CardRank.ten:
                    return 10;
                case CardRank.jack:
                    return 11;
                case CardRank.queen:
                    return 12;
                case CardRank.king:
                    return 13;
            }
        }
    }
}
