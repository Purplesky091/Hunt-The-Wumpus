/*
	Hunt The Wumpus
	Written by Lawrence Hsia
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hunt_the_Wumpus_Text_based
{
    class Game
    {
        private World world;
        private Player player;
        private Wumpus wumpus;

        /// <summary>
        /// Contains elements that should only be initialized once,
        /// even when the game is restarted
        /// </summary>
        public Game()
        {
        }

        /// <summary>
        /// Elements that need to be initialized any time the game restartss
        /// </summary>
        private void Initialize()
        {
            world = new World();
            player = new Player(0);
            wumpus = new Wumpus(world.InitialWumpusRoom);
        }

        private void ResetVariables(bool resetRooms)
        {
            world.ResetWorld(resetRooms);
            player = new Player(0);
            wumpus = new Wumpus(world.InitialWumpusRoom);
        }

        /// <summary>
        /// Main game loop
        /// </summary>
        public void Run()
        {
            Initialize();

            do
            {
                player.CheckForHazards();
                player.CheckFor(wumpus);
                Console.WriteLine();
                while (Player.IsAlive && !Player.HasWon)
                {
                    Update();
                }
 
            } while (WillRestart());

        }

        private void Update()
        {
            ExecuteWumpusTurn();
            ExecutePlayerTurn();       
        }

        private void ExecutePlayerTurn()
        {
            if (Player.IsDead)
                return;

            string playerInput;
            bool willReprompt = false;

            do
            {
                Console.WriteLine("You are in room " + player.CurrRoom);
                Console.Write("Tunnels lead to ");
                player.ListConnectedRooms();
                Console.Write("Shoot, Move, or Quit (S-M-Q)? ");
                playerInput = Console.ReadLine();

                switch (playerInput.ToLower())
                {
                    case "s":
                        bool hasArrows = player.Shoot(wumpus); //try to shoot at wumpus
                        willReprompt = hasArrows ? false : true;
                        break;
                    case "m":
                        player.Move();
                        player.CheckFor(wumpus);
                        player.CheckForHazards();
                        willReprompt = false;
                        break;
                    case "q":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid command\n");
                        willReprompt = true;
                        break;
                }

                Console.WriteLine();

            } while (willReprompt);
        }

        private void ExecuteWumpusTurn()
        {
            if (Player.HasWon || wumpus.IsDead)
                return;
            else if (wumpus.IsAwake)
            {
                wumpus.Move();
                wumpus.CheckForPlayer(player);
            }
        }

        private bool WillRestart()
        {
            bool invalidResponse = false;
            bool willRestart = false;
            bool resetHazardLocations = false;

            string endingText = Player.HasWon ? "You are weiner!" : "Game Over";
            Console.WriteLine(endingText);
            do
            {
                Console.Write("Restart? (y/n):");
                string restartResponse = Console.ReadLine();
                switch (restartResponse.ToLower())
                {
                    case "y":
                        willRestart = true;
                        invalidResponse = false;
                        break;
                    case "n":
                        willRestart = false;
                        invalidResponse = false;
                        break;
                    default:
                        invalidResponse = true;
                        break;
                }

            } while (invalidResponse);

            do
            {
                Console.Write("Reset wumpus, bats, and pits locations (y/n):");
                string restartResponse = Console.ReadLine();
                switch (restartResponse.ToLower())
                {
                    case "y":
                        invalidResponse = false;
                        resetHazardLocations = true;
                        break;
                    case "n":
                        invalidResponse = false;
                        resetHazardLocations = false;
                        break;
                    default:
                        invalidResponse = true;
                        break;
                }

            } while (invalidResponse);

            ResetVariables(resetHazardLocations);

            return willRestart;
        }
    }
}
