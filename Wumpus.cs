using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus_Text_based
{
    class Wumpus
    {
        public bool IsAwake { get; set; }
        public bool IsDead { get; private set; }
        public Room CurrRoom { get; private set; }
        private Random random;

        public Wumpus(int currRoomID)
        {
            CurrRoom = World.GetRoomById(currRoomID);
            IsAwake = false;
            IsDead = false;
            random = new Random();
        }

        public void Move()
        {
            if (random.Next(0, 100) < 75)
            {
                int randomIndex = random.Next(0, 2);
                CurrRoom = CurrRoom.AdjList[randomIndex];
            }
        }

        public void Attack(Player player)
        {
            if (IsAwake)
            {
                Console.WriteLine("Wumpus ate player");
                player.Die();
            }
            else
            {
                Console.WriteLine("Oops, bumped a wumpus");
                IsAwake = true;
            }

        }

        public void CheckForPlayer(Player player)
        {
            if (player.CurrRoom.Id == CurrRoom.Id)
            {
                Attack(player);
            }
        }

        public void PrintWarning()
        {
            Console.WriteLine("I smell a Wumpus");
        }

        public void Die()
        {
            IsDead = true;
            IsAwake = false;
            Player.HasWon = true;
        }

    }
}
