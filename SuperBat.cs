using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus_Text_based
{
    class SuperBat: IHazard
    {
        private Random roomGenerator = new Random();

        public void Attack(Player player)
        {
            int roomToMoveTo = roomGenerator.Next(0, 19);
            player.TravelTo(roomToMoveTo);
            Console.WriteLine("Aha, superbat attack. Elsewhereville for you!");
        }

        public bool IsBlank()
        {
            return false;
        }


        public void PrintWarning()
        {
            Console.WriteLine("Bats nearby");
        }
    }
}
