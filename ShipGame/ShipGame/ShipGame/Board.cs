using System;
using System.Collections.Generic;

namespace ShipGame
{
    internal class Board
    {
        private char[,] ownGrid;
        private char[,] targetGrid;
        private List<Ship> ships;

        public Board()
        {
            ownGrid = new char[10, 10];
            targetGrid = new char[10, 10];
            ships = new List<Ship>();
            InitializeGrids();
        }

        private void InitializeGrids()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    ownGrid[i, j] = ' ';
                    targetGrid[i, j] = ' ';
                }
            }
        }

        public void ManualPlaceShips()
        {
            Console.WriteLine("Ship placement on the board:");
            foreach (var size in new int[] {4, 3, 3, 2, 2, 2, 1, 1, 1, 1 })
            {
                Console.Clear();
                DisplayOwnBoard();
                Console.WriteLine($"Place a ship of size {size}:");
                bool shipPlaced = false;
                while (!shipPlaced)
                {
                    Console.Write("Enter the starting coordinate (For example A1): ");
                    string startCoord = Console.ReadLine().ToUpper();
                    Point startPoint = new Point(startCoord);

                    if (!IsWithinBounds(startPoint))
                    {
                        Console.WriteLine("Invalid starting coordinate. Please enter a valid coordinate on the board.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        continue;
                    }

                    if (IsOccupied(startPoint))
                    {
                        Console.WriteLine("There is already a ship placed in this location. Choose a different starting possition.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        continue;
                    }

                    if(IsShipAround(startPoint.X, startPoint.Y, new List<Point> { new Point(-10, -10) }))
                    {
                        Console.WriteLine("There is already a ship placed around. Choose a different starting possition.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        continue;
                    }
                    if (size == 1)
                    {
                        PlaceSingleShip(startPoint);
                        shipPlaced = true;
                    }
                    else
                    {
                        Console.Write("Enter the direction (H for horizontal, V for vertical): ");
                        char direction = Console.ReadKey().KeyChar;
                        Console.WriteLine();
                        if (PlaceShip(new Ship(size), startPoint, direction))
                        {
                            shipPlaced = true;
                        }
                    }
                }
            }
            Console.WriteLine("All ships placed!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private bool IsShipAround(int x, int y, List<Point> curShip)
        {         
            for(int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    bool checkGrid = true;
                    foreach(Point shipPart in curShip)
                    {
                        if( shipPart.X == x+i && shipPart.Y == y+j) checkGrid = false;                               
                    }

                    if (x + i >= 0 && x + i <= 9 && y + j >= 0 && y + j <= 9)
                    {
                        if (ownGrid[x + i, y + j] == 'S' && checkGrid) return true;
                    }
                }
            }
            return false;
        }
        private void PlaceSingleShip(Point startPoint)
        {
            ownGrid[startPoint.X, startPoint.Y] = 'S';
            ships.Add(new Ship(1) { OccupiedCells = { startPoint } });
        }

        private bool IsWithinBounds(Point point)
        {
            return point.X >= 0 && point.X < 10 && point.Y >= 0 && point.Y < 10;
        }

        private bool IsOccupied(Point point)
        {
            return ownGrid[point.X, point.Y] == 'S';
        }

        private bool PlaceShip(Ship ship, Point startPoint, char direction)
        {
            int x = startPoint.X;
            int y = startPoint.Y;
            List<Point> curShip = new List<Point>();
            curShip.Add(startPoint);

            if (!IsWithinBounds(startPoint))
            {
                Console.WriteLine("Invalid starting coordinate. Please enter a valid coordinate on the board.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return false;
            }

            if ((direction != 'H' && direction != 'h' && direction != 'V' && direction != 'v'))
            {
                Console.WriteLine("Wrong direction. Please choose a correct direction.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return false;
            }

            if ((direction == 'H' || direction == 'h') && y + ship.Size > 10)
            {
                Console.WriteLine("Ship placement goes out the board's boundaries. Please choose a different starting position or direction.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return false;
            }

            if ((direction == 'V' || direction == 'v') && (x + ship.Size > 10 || y < 0 || y >= 10))
            {
                Console.WriteLine("Ship placement goes out the board's boundaries. Please choose a different starting position or direction.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return false;
            }

            ownGrid[x, y] = 'S';
            ship.AddCell(new Point(x, y));
            ships.Add(ship);
            curShip.Add(new Point(x, y));

            for (int i = 1; i < ship.Size; i++)
            {
                if (direction == 'V' || direction == 'v')
                {
                    if (!IsWithinBounds(new Point(x + i, y)))
                    {
                        Console.WriteLine("Ship placement goes out the board's boundaries. Please choose a different starting position or direction.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        return false;
                    }
                    if (IsShipAround(x + i, y, curShip))
                    {
                        Console.WriteLine("There is already a ship placed around. Please choose a different starting position or direction.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        for(int j=0; j < ships[ships.Count-1].OccupiedCells.Count; j++)
                        {
                            ownGrid[ships[ships.Count - 1].OccupiedCells[j].X, ships[ships.Count - 1].OccupiedCells[j].Y] = ' ';
                        }
                        ships.RemoveAt(ships.Count - 1);
                        return false;
                    }
                    ownGrid[x + i, y] = 'S';
                    ship.AddCell(new Point(x + i, y));
                    curShip.Add(new Point(x + i, y));
                }
                else if (direction == 'H' || direction == 'h')
                {
                    if (!IsWithinBounds(new Point(x, y + i)))
                    {
                        Console.WriteLine("Ship placement goes out the board's boundaries. Please choose a different starting position or direction.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        return false;
                    }
                    if (IsShipAround(x, y + i, curShip))
                    {
                        Console.WriteLine("There is already a ship placed around. Please choose a different starting position or direction.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        for (int j = 0; j < ships[ships.Count - 1].OccupiedCells.Count; j++)
                        {
                            ownGrid[ships[ships.Count - 1].OccupiedCells[j].X, ships[ships.Count - 1].OccupiedCells[j].Y] = ' ';
                        }
                        ships.RemoveAt(ships.Count - 1);
                        return false;
                    }
                    ownGrid[x, y + i] = 'S';
                    ship.AddCell(new Point(x, y + i));
                    curShip.Add(new Point(x, y + i));
                }
            }

            return true;
        }

        public bool IsHit(Point target)
        {
            return ownGrid[target.X, target.Y] == 'S';
        }

        public bool IsShipSunk(Point target)
        {
            foreach (var ship in ships)
            {
                if (ship.IsHit(target) && ship.IsSunk())
                    return true;
            }
            return false;
        }

        public void MarkHit(Point target)
        {
            targetGrid[target.X, target.Y] = 'X';
        }

        public void MarkMiss(Point target)
        {
            targetGrid[target.X, target.Y] = 'O';
        }
        public void SunkShip()
        {
            for(int i=0; i<ships.Count; i++)
            {
                if (ships[i].OccupiedCells.Count == 0)
                {
                    ships.Remove(ships[i]);
                    Console.WriteLine("You sunk a ship!");
                }
            }          
        }

        public void DisplayOwnBoard()
        {
            Console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
            for (int i = 0; i < 10; i++)
            {
                Console.Write((char)('A' + i) + " ");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(ownGrid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void DisplayTargetBoard()
        {
            Console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
            for (int i = 0; i < 10; i++)
            {
                Console.Write((char)('A' + i) + " ");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(targetGrid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public bool AllShipsSunk()
        {
            return ships.Count == 0;
        }

        public void RemoveShipPart(Point target)
        {
            foreach (var part in ships)
            {
                part.RemoveHitCell(target);
            }
        }
    }
}
