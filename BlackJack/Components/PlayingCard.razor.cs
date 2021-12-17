using BlackJack.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Components
{
    public partial class PlayingCard : ComponentBase 
    {
        [Parameter]
        public Card Card { get; set; }
    }
}
