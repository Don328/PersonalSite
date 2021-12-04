using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Utils
{
    internal static class WagerValidator
    {
        static int min = 10;
        static int increment = 10;
        internal const string successMessage = "success";

        internal static string IsValid(int wager)
        {
            if (wager < min)
            {
                return "Minimum wager value is 10";
            }
            if (wager % increment != 0)
            {
                return "Wager must be in increments of 10";
            }

            return successMessage;
        }
    }
}
