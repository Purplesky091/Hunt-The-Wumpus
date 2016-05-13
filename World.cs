using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hunt_the_Wumpus_Text_based
{
    /// <summary>
    /// A graph of rooms. 
    /// </summary>
    class World
    {
        private static Room [] roomNodes = new Room[GameConstants.WORLD_SIZE];//the list of nodes. The "graph"
        private HashSet<int> batRooms = new HashSet<int>(); // set of rooms bats can be in
        private HashSet<int> pitRooms = new HashSet<int>(); // set of rooms pits can be in
        public int InitialWumpusRoom {get; private set;}
        private Random random = new Random();

        public World() 
        {

            for (int i = 0; i < GameConstants.WORLD_SIZE; i++)
            {
                roomNodes[i] = new Room(i);
            }

            BuildMap();
            GenerateBatRoomNumbers();
            GeneratePitRoomNumbers();
            InitialWumpusRoom = GenerateWumpusRoomNumber();
            SpawnHazards();
        }

        public void ResetWorld(bool resetRooms)
        {
            for (int i = 0; i < GameConstants.WORLD_SIZE; i++)
            {
                roomNodes[i].Hazard = GameConstants.BLANK_HAZARD;
            }

            if (resetRooms)
            {
                GenerateBatRoomNumbers();
                GeneratePitRoomNumbers();
                InitialWumpusRoom = GenerateWumpusRoomNumber();
            }

            SpawnHazards();
        }

        private void SpawnHazards()
        {
            SuperBat superBat = new SuperBat();
            Pit pit = new Pit();

            foreach (int room in batRooms)
            {
                roomNodes[room].Hazard = superBat;
            }

            foreach (int pitRoom in pitRooms)
            {
                roomNodes[pitRoom].Hazard = pit;
            }
        }

        private void GenerateBatRoomNumbers()
        {
            batRooms.Clear();
            int numOfRooms = 3;

            while (batRooms.Count < numOfRooms)
            {
                batRooms.Add(random.Next(1, 19));
            }
        }

        private void GeneratePitRoomNumbers()
        {
            pitRooms.Clear();
            int numOfRooms = 2;

            while (pitRooms.Count < numOfRooms)
            {
                int randomRoom = random.Next(1, 19);
                if (batRooms.Contains(randomRoom))
                    continue; //regenerate random room number
                else
                    pitRooms.Add(randomRoom);
            }
        }

        /// <summary>
        /// Generates a random room for wumpus to be in. <para/>
        /// Ensures that wumpus does not start in the same room as other hazards.
        /// </summary>
        /// <returns></returns>
        private int GenerateWumpusRoomNumber()
        {
            int randomNumber = random.Next(1, 19); // must be room 1 because player always starts in room 0

            while (batRooms.Contains(randomNumber) && pitRooms.Contains(randomNumber))
            {
                randomNumber = random.Next(1, 19);
            }

            return randomNumber;
        }

        /// <summary>
        /// Connects room nodes <para/>
        /// as a dodecahedron
        /// </summary>
        private void BuildMap()
        {
            roomNodes[0].ConnectRoom(roomNodes[1]);
            roomNodes[0].ConnectRoom(roomNodes[2]);
            roomNodes[0].ConnectRoom(roomNodes[10]);

            roomNodes[1].ConnectRoom(roomNodes[0]);
            roomNodes[1].ConnectRoom(roomNodes[4]);
            roomNodes[1].ConnectRoom(roomNodes[15]);

            roomNodes[2].ConnectRoom(roomNodes[0]);
            roomNodes[2].ConnectRoom(roomNodes[3]);
            roomNodes[2].ConnectRoom(roomNodes[6]);

            roomNodes[3].ConnectRoom(roomNodes[2]);
            roomNodes[3].ConnectRoom(roomNodes[4]);
            roomNodes[3].ConnectRoom(roomNodes[5]);

            roomNodes[4].ConnectRoom(roomNodes[1]);
            roomNodes[4].ConnectRoom(roomNodes[3]);
            roomNodes[4].ConnectRoom(roomNodes[9]);

            roomNodes[5].ConnectRoom(roomNodes[3]);
            roomNodes[5].ConnectRoom(roomNodes[7]);
            roomNodes[5].ConnectRoom(roomNodes[8]);
            
            roomNodes[6].ConnectRoom(roomNodes[2]);
            roomNodes[6].ConnectRoom(roomNodes[7]);
            roomNodes[6].ConnectRoom(roomNodes[11]);

            roomNodes[7].ConnectRoom(roomNodes[5]);
            roomNodes[7].ConnectRoom(roomNodes[6]);
            roomNodes[7].ConnectRoom(roomNodes[12]);

            roomNodes[8].ConnectRoom(roomNodes[5]);
            roomNodes[8].ConnectRoom(roomNodes[9]);
            roomNodes[8].ConnectRoom(roomNodes[13]);

            roomNodes[9].ConnectRoom(roomNodes[4]);
            roomNodes[9].ConnectRoom(roomNodes[8]);
            roomNodes[9].ConnectRoom(roomNodes[14]);

            roomNodes[10].ConnectRoom(roomNodes[0]);
            roomNodes[10].ConnectRoom(roomNodes[11]);
            roomNodes[10].ConnectRoom(roomNodes[19]);

            roomNodes[11].ConnectRoom(roomNodes[6]);
            roomNodes[11].ConnectRoom(roomNodes[10]);
            roomNodes[11].ConnectRoom(roomNodes[16]);

            roomNodes[12].ConnectRoom(roomNodes[7]);
            roomNodes[12].ConnectRoom(roomNodes[13]);
            roomNodes[12].ConnectRoom(roomNodes[16]);

            roomNodes[13].ConnectRoom(roomNodes[8]);
            roomNodes[13].ConnectRoom(roomNodes[12]);
            roomNodes[13].ConnectRoom(roomNodes[17]);

            roomNodes[14].ConnectRoom(roomNodes[9]);
            roomNodes[14].ConnectRoom(roomNodes[15]);
            roomNodes[14].ConnectRoom(roomNodes[17]);

            roomNodes[15].ConnectRoom(roomNodes[1]);
            roomNodes[15].ConnectRoom(roomNodes[14]);
            roomNodes[15].ConnectRoom(roomNodes[19]);

            roomNodes[16].ConnectRoom(roomNodes[11]);
            roomNodes[16].ConnectRoom(roomNodes[12]);
            roomNodes[16].ConnectRoom(roomNodes[18]);

            roomNodes[17].ConnectRoom(roomNodes[13]);
            roomNodes[17].ConnectRoom(roomNodes[14]);
            roomNodes[17].ConnectRoom(roomNodes[18]);

            roomNodes[18].ConnectRoom(roomNodes[16]);
            roomNodes[18].ConnectRoom(roomNodes[17]);
            roomNodes[18].ConnectRoom(roomNodes[19]);

            roomNodes[19].ConnectRoom(roomNodes[10]);
            roomNodes[19].ConnectRoom(roomNodes[15]);
            roomNodes[19].ConnectRoom(roomNodes[18]);
        }

        public static Room GetRoomById(int id)
        {
            if (id < GameConstants.WORLD_SIZE)
                return roomNodes[id];
            else
                throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Checks if two rooms are connected.
        /// </summary>
        /// <param name="roomID1"></param>
        /// <param name="roomID2"></param>
        /// <returns></returns>
        public static bool AreRoomsConnected(int roomID1, int roomID2)
        {
            return roomNodes[roomID1].IsInAdjList(roomID2);
        }
    }
}
