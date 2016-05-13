using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hunt_the_Wumpus_Text_based
{
    class Room
    {
        public int Id { get; private set; }
        public IHazard Hazard {get; set;}
        public List<Room> AdjList { get; private set; }//3 connections per room. 6 capacity is to make sure that the adjacency list doesn't resize itself.
        public bool isVisited { get; set; } // used primarily for shooting code

        /// <summary>
        /// makes a new room with the given id #.<para/>
        /// Will give the room a "Blank hazard" by default.
        /// </summary>
        /// <param name="id"></param>
        public Room(int id) 
        {
            Id = id;
            Hazard = GameConstants.BLANK_HAZARD;
            AdjList = new List<Room>(6);
            isVisited = false;
        }

        /// <summary>
        /// Connects another room node to the room object.
        /// </summary>
        /// <param name="room"> The room that is to be connected </param>
        public void ConnectRoom(Room room)
        {
            AdjList.Add(room);
        }

        public void PrintConnectedRooms()
        {            
            for (int i = 0; i < AdjList.Count; i++ )
            {
                Console.Write(AdjList[i].Id + " ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Checks if a room id is in this room's adjacency list <para/>
        /// <returns>true</returns> if room id is in adjacency list <para/>
        /// <returns>false</returns> if room id is missing from adjacency list
        /// </summary>
        /// <param name="roomID">the room id you wish to see is present or not in adjacency list</param>
        public bool IsInAdjList(int roomID)
        {
            for (int i = 0; i < AdjList.Count; i++)
            {
                if (roomID == AdjList[i].Id)
                    return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public bool HasHazard()
        {
            if (Hazard.IsBlank())
                return false;
            else
                return true;
        }


        /// <summary>
        /// Will print adjacent rooms' warning messages.<para/>
        /// Will print nothing if adjacent rooms has no hazards.
        /// </summary>
        public void PrintAdjRoomWarnings()
        {
            for (int i = 0; i < AdjList.Count; i++)
            {
                if (AdjList[i].HasHazard())
                {
                    AdjList[i].Hazard.PrintWarning();
                }
            }

        }

        public void PrintAdjRoomWarningFor(Wumpus wumpus)
        {
            for (int i = 0; i < AdjList.Count; i++)
            {
                if (AdjList[i].Id == wumpus.CurrRoom.Id)
                {
                    wumpus.PrintWarning();
                }
            }
        }

        /// <summary>
        /// Checks adjacency lists hazards
        /// </summary>
        /// <returns>true if connected rooms have hazards</returns> 
        /// <returns>false if the connected rooms do not have hazards</returns> 
        public bool CheckAdjListHazards()
        {
            for (int i = 0; i < AdjList.Count; i++)
            {
                if (AdjList[i].HasHazard())
                {
                    return true;
                }
                    
            }

            return false;
        }

        /// <summary>
        /// Gets a connected, unvisited node
        /// </summary>
        /// <returns>The room id of connected, unvisited node</returns>
        public int GetUnvisitedNode()
        {
            for (int i = 0; i < AdjList.Count; i++)
            {
                if (AdjList[i].isVisited)
                    continue;
                else
                {
                    AdjList[i].isVisited = true;
                    return AdjList[i].Id;
                }
            }

            return -1;
        }
    }
}
