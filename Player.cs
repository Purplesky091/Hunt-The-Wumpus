using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus_Text_based
{
    public enum ParseError { ABAPattern, NotNumber, InvalidNumber, Correct, Failure, ListTooLong, RepeatedNumber }

    class Player
    {
        public Room CurrRoom { get; private set; }
        public static bool IsAlive { get; private set; }
        public static bool IsDead // convenience variable that makes logic easier to understand.
        {
            get { return !IsAlive; }
            set
            {
                IsAlive = !value;
            }
        }

        public static bool HasWon { get; set; }
        private Stack<Arrow> arrows = new Stack<Arrow>();
        int numOfArrows = 5;

        public Player(int startingRoom)
        {
            CurrRoom = World.GetRoomById(startingRoom);
            IsAlive = true;
            HasWon = false;
            for (int i = 0; i < numOfArrows; i++)
            {
                arrows.Push(new Arrow());
            }
        }

        /// <summary>
        /// Convenience method that will list 
        /// possible rooms that a player can visit
        /// </summary>
        public void ListConnectedRooms()
        {
            CurrRoom.PrintConnectedRooms();
        }

        public void Move()
        {
            string playerInput;
            int roomNum = 0;

            //ask player where they wish to move
            do
            {
                Console.Write("Where to?: ");
                playerInput = Console.ReadLine();

            } while (!IsInputValid(playerInput, ref roomNum));

            Console.WriteLine();
            TravelTo(roomNum);
        }

        private bool IsInputValid(string playerInput, ref int roomNum)
        {
            //check if player entered in valid number.
            if (Int32.TryParse(playerInput, out roomNum) && CurrRoom.IsInAdjList(roomNum))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Not Possible");
                return false;
            }
        }

        /// <summary>
        /// Will take the player to the specified room using the room ID number
        /// </summary>
        /// <param name="room">the roomID number of room that player should travel to</param>
        public void TravelTo(int roomID)
        {
            CurrRoom = World.GetRoomById(roomID);
        }

        /// <summary>
        /// Checks if nearby rooms have hazards
        /// Also has a hazard attack a player if the player runs into a hazard
        /// </summary>
        public void CheckForHazards()
        {
            if (CurrRoom.HasHazard())
            {
                CurrRoom.Hazard.Attack(this);
            }
            else if(IsAlive)
            {
                CurrRoom.PrintAdjRoomWarnings();
            }
        }

        /// <summary>
        /// Will check if the wumpus is in the same room as you. <para/>
        /// If it is, then the wumpus will attack the player <para/>
        /// If the wumpus is in an adjacent room, a warning will be printed <para/>
        /// Otherwise, no warning will be printed.
        /// </summary>
        /// <param name="wumpus"></param>
        public void CheckFor(Wumpus wumpus)
        { 
            if (CurrRoom.Id == wumpus.CurrRoom.Id)
            {
                wumpus.Attack(this);
            }
            else if(IsAlive)
            {
                CurrRoom.PrintAdjRoomWarningFor(wumpus);
            }
        }

        /// <summary>
        /// shoots an arrow through the rooms player inputs
        /// </summary>
        /// <param name="wumpus"></param>
        /// <returns>if shooting was successful or not</returns>
        public bool Shoot(Wumpus wumpus)
        {
            bool inputIsRight = true;
            LinkedList<int> roomIDs = new LinkedList<int>();

            if (arrows.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("All arrows used up!");
                return false;
            }
                
            Arrow arrow = arrows.Pop();
            do
            {
                Console.Write("Insert rooms (seperated by spaces): ");
                string input = Console.ReadLine();
                inputIsRight = TryParse(input, roomIDs);

            } while (!inputIsRight);

            arrow.FlyAcrossPath(roomIDs, wumpus);
            return true;
        }

        /// <summary>
        /// Takes the rooms numbers player inputted into console <para></para>
        /// and adds the room numbers to the roomIDs list.
        /// </summary>
        /// <param name="input"> string that player input </param>
        /// <param name="roomIDs"> the List of integers that will hold all the roomIDs </param>
        /// <returns>true if parsing was successful</returns>
        /// <returns>false if parsing failed.</returns>
        private bool TryParse(string input, LinkedList<int> roomIDs)
        {
            char[] splitChars = { ' ' };
            string[] inputRooms = input.Split(splitChars);

            ParseError parseError = FindParseError(inputRooms, roomIDs);
            switch (parseError)
            {
                case ParseError.ABAPattern:
                    Console.WriteLine("Arrows are not that crooked");
                    goto case ParseError.Failure;

                case ParseError.InvalidNumber:
                    Console.WriteLine("Room numbers must be between 0 and 19");
                    goto case ParseError.Failure;

                case ParseError.NotNumber:
                    Console.WriteLine("Input must be numbers");
                    goto case ParseError.Failure;

                case ParseError.ListTooLong:
                    Console.WriteLine("Arrow cannot fly to more than 5 rooms");
                    goto case ParseError.Failure;

                case ParseError.RepeatedNumber:
                    Console.WriteLine("Rooms cannot be repeated");
                    goto case ParseError.Failure;

                default:
                    Console.WriteLine("Forgot to handle this error");
                    goto case ParseError.Failure;

                case ParseError.Failure:
                    roomIDs.Clear();
                    return false;

                case ParseError.Correct:
                    break;
            }

            CorrectInput(roomIDs);
            return true;
        }

        /// <summary>
        /// Finds ParseErrors within user input. <para/>
        /// Will return "correct" if user input is correct.
        /// </summary>
        /// <param name="inputRooms"></param>
        /// <returns>"correct" if user input is correct.</returns>
        private ParseError FindParseError(string[] inputRooms, LinkedList<int> roomIDs)
        {
            ParseError result = ParseError.Correct;

            if (inputRooms.Length > 5)
                return ParseError.ListTooLong;

            for (int i = 0; i < inputRooms.Length; i++)
            {
                int currentElement;

                if (Int32.TryParse(inputRooms[i], out currentElement))
                {
                    if (currentElement < 0 || currentElement >= GameConstants.WORLD_SIZE)
                    {
                        result = ParseError.InvalidNumber;
                        break;
                    }

                    else if (World.GetRoomById(currentElement).isVisited)
                    {
                        //check if current element equals the last element parsed.
                        if (currentElement == roomIDs.Last())
                        {
                            result = ParseError.RepeatedNumber;
                        }
                        else
                        {
                            result = ParseError.ABAPattern;
                        }
                        
                        break;
                    }
                    else
                    {
                        World.GetRoomById(currentElement).isVisited = true;
                        roomIDs.AddLast(currentElement);
                    }

                }
                else
                {
                    result = ParseError.NotNumber;
                    break;
                }
            }

            // unvisit all rooms that were added to the roomIDs list
            // if a ParseError happened
            if (result != ParseError.Correct)
            {
                foreach (int i in roomIDs)
                {
                    World.GetRoomById(i).isVisited = false;
                }
            }
           
            return result;
        }

        /// <summary>
        /// Ensures that the list of rooms are all connected. <para/>
        /// If two rooms are not connected, this method will overwrite the second room <para/>
        /// with one of the first room's neighbors.
        /// </summary>
        /// <param name="roomIDs"></param>
        private void CorrectInput(LinkedList<int> roomIDs)
        {
            //start on the 2nd element
            for (LinkedListNode<int> currentNode = roomIDs.First.Next; currentNode != null; currentNode = currentNode.Next)
            {
                int current = currentNode.Value;
                int previous = currentNode.Previous.Value;

                if (World.AreRoomsConnected(previous, current))
                {
                    continue;
                }
                else
                {
                    /*
                     * Whenever the CorrectInput() method receives the roomIDs list,
                     * all rooms in the roomIDs list are marked as visited.
                     * If the current node needs to be changed to another room, and its just straight up deleted,
                     * the current node is still marked as "visited' whenever it gets dropped from the list.
                     * Unvisit the current node in order to ensure that doesn't happen.
                     */
                    World.GetRoomById(currentNode.Value).isVisited = false;
                    currentNode.Value = World.GetRoomById(previous).GetUnvisitedNode();//change current to one of previous's unvisited connections
                }
            }
        }

        public void Die()
        {
            IsAlive = false;
        }
    }
}
