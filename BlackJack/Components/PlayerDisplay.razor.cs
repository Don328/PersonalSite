using BlackJack.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Components
{
    public partial class PlayerDisplay : ComponentBase
    {

        [Parameter]
        public Player Player { get; set; }
            = new(200);
    }
}
