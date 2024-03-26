using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipGame
{
    internal class Game
    {
        private Player player1;
        private Player player2;
        private Player currentPlayer;
        private Player winner;

        public void Start()
        {
            player1 = new Player();
            player2 = new Player();
            currentPlayer = player1;
            winner = null;

            Console.WriteLine("Welcome to Battleship Game!");

            SetupPlayers();

            while (winner == null)
            {
                Console.Clear();
                Console.WriteLine($"\n It's {currentPlayer.Name}'s turn:");
                currentPlayer.PlayTurn(currentPlayer == player1 ? player2 : player1);
                currentPlayer = currentPlayer == player1 ? player2 : player1;
                CheckForWinner();
            }
        }

        private void SetupPlayers()
        {
            Console.WriteLine("Setting up players...");
            Console.WriteLine("Player 1, please set up your ships:");
            player1.SetupShips();
            Console.WriteLine("Player 2, please set up your ships:");
            player2.SetupShips();
            Console.WriteLine("Players are ready!");
            Console.WriteLine("Press any key to start the game...");
            Console.ReadKey();
        }

        private void CheckForWinner()
        {
            if (player1.HasLost())
            {
                winner = player2;
            }
            else if (player2.HasLost())
            {
                winner = player1;
            }
            else
            {
                winner = null;
            }

            if (winner != null)
            {
                Console.WriteLine($"Congratulations {winner.Name}! You won the game!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0); 
            }
        }
    }
}
