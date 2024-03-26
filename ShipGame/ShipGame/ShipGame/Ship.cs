using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipGame
{
    internal class Ship
    {
        public List<Point> OccupiedCells { get; private set; }
        public int Size { get; private set; }

        public Ship(int size)
        {
            Size = size;
            OccupiedCells = new List<Point>();
        }

        public void AddCell(Point cell)
        {
            OccupiedCells.Add(cell);
        }

        public bool IsHit(Point target)
        {
            return OccupiedCells.Contains(target);
        }

        public bool IsSunk()
        {
            return OccupiedCells.Count == 0;
        }

        public void RemoveHitCell(Point target)
        {
            var cellToRemove = OccupiedCells.FirstOrDefault(cell => cell.X == target.X && cell.Y == target.Y);
            if (cellToRemove != null)
            {
              OccupiedCells.Remove(cellToRemove);
            }
        }
    }
}
