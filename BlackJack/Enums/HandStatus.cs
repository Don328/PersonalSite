using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Enums
{
    public enum HandStatus
    {
        New,
        Dealt,
        BlackJack,
        Busted,
        Active,
        Pending,
        Hold,
        Paid,
    }
}
