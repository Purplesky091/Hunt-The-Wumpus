using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus_Text_based
{
    class Arrow
    {
        public void FlyAcrossPath(LinkedList<int> roomIDs, Wumpus wumpus)
        {
            bool hitWumpus = false;
            Console.WriteLine();
            Console.Write("Arrow travled to rooms ");

            foreach (int roomID in roomIDs)
            {
                
                Console.Write(roomID + " ");
                World.GetRoomById(roomID).isVisited = false;//make arrow "unvisit" nodes
                
                if (roomID == wumpus.CurrRoom.Id)
                {
                    hitWumpus = true;
                    wumpus.Die();
                }
                 
            }

            Console.WriteLine();
            if (hitWumpus)
            {
                Console.WriteLine("Wumpus is dead");
            }
            else
            {
                Console.WriteLine("Missed!");
                if (!wumpus.IsAwake)
                {
                    Console.WriteLine("Wumpus woke up");
                    wumpus.IsAwake = true;
                }
            }
        }
    }
}
