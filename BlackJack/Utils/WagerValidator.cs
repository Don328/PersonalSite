using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Utils
{
    internal class WagerValidator
    {

        static int min = 10;
        static int increment = 10;
        private string result = string.Empty;
        private readonly string success = "Success";
        private readonly string lessThanMin = $"Minimum wager value is {min}";
        private readonly string notCorrectIncrement = $"Wager must be in increments of {increment}";

        public WagerValidator(int wager)
        {
            

            if (wager < min)
            {
                result = lessThanMin;
            }
            else if (wager % increment != 0)
            {
                result = notCorrectIncrement;
            }
            else
            {
                result = success;
            }
        }

        public string Success { get { return success; } }
        public string Result { get { return result; } }
    }
}
