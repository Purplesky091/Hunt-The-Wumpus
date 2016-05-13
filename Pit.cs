using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus_Text_based
{
    class Pit: IHazard
    {
        /// <summary>
        /// instantly kills the player.
        /// </summary>
        /// <param name="player"></param>
        public void Attack(Player player)
        {
            Console.WriteLine("YIEEEEE... fell in a pit");
            player.Die();
        }

        public bool IsBlank()
        {
            return false;
        }


        public void PrintWarning()
        {
            WarningPrinter.Add("I feel a draft");
        }
    }
}
