using AlgoCards.Models;
using BlackJack.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Components
{
    public partial class DealerDisplay : ComponentBase
    {
        [Parameter]
        public DealerHand Hand { get; set; }
            = new DealerHand();

        private Card GetDealerShow()
        {
            return Hand.Cards[0];
        }
    }
}
