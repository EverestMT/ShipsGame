using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipGame
{
    internal class Player
    {
        public string Name { get; private set; }
        private Board ownBoard;
        private Board targetBoard;
        private List<Point> targetsHistory;

        public Player()
        {
            targetsHistory = new List<Point>();
        }

        public void SetupShips()
        {
            Console.Write("Enter player's name: ");
            Name = Console.ReadLine();
            ownBoard = new Board();
            ownBoard.ManualPlaceShips();
            targetBoard = new Board();
        }

        public void PlayTurn(Player opponent)
        {
            Console.WriteLine("Your board:");
            ownBoard.DisplayOwnBoard();
            Console.WriteLine("Opponent's board:");
            targetBoard.DisplayTargetBoard();

            bool endLoop = false;
            do
            {
                Console.Write("Enter target coordinates: ");
                string target = Console.ReadLine().ToUpper();
                Point targetPoint = new Point(target);
                while (targetsHistory.Contains(targetPoint))
                {
                    Console.WriteLine("You've already shot there. Choose a different target.");
                    Console.Write("Enter target coordinates: ");
                    target = Console.ReadLine().ToUpper();
                    targetPoint = new Point(target);
                }
                targetsHistory.Add(targetPoint);
                if (opponent.ownBoard.IsHit(targetPoint))
                {
                    Console.WriteLine("It's a hit!");
                    targetBoard.MarkHit(targetPoint);
                    opponent.ownBoard.RemoveShipPart(targetPoint);
                    opponent.ownBoard.SunkShip();                    

                    ownBoard.MarkHit(targetPoint);                    
                }
                else
                {
                    Console.WriteLine("Miss!");
                    targetBoard.MarkMiss(targetPoint);
                    ownBoard.MarkMiss(targetPoint);  
                    endLoop = true;
                }

                if (opponent.HasLost())
                {
                    Console.WriteLine($"Congratulations {Name}! You won the game!");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(0); 
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("Your board:");
                ownBoard.DisplayOwnBoard();
                Console.WriteLine("Opponent's board:");
                targetBoard.DisplayTargetBoard();
                

            } while (!endLoop);
        }

        public bool HasLost()
        {
            return ownBoard.AllShipsSunk();
        }
    }
}

