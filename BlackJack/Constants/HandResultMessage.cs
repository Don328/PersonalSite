using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Constants
{
    public static class HandResultMessage
    {
        public static readonly string hold = string.Empty;
        public const string blackJack = "BlackJack";
        public const string busted = "Busted";
        public const string dealerWins = "Dealer Wins";
        public const string pending = "Waiting for dealer";
        public const string push = "Push";
        public const string playerWins = "Player Wins!";
        public const string dealerHolds = "Dealer Holds";
        public const string dealerBlackJack = "Dealer BlackJack";
    }
}
