using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus_Text_based
{
    interface IHazard
    {
        void Attack(Player player);
        bool IsBlank();
        void PrintWarning();
    }
}
