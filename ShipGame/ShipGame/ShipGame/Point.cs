using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipGame
{
    internal class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(string input)
        {
            X = input[0] - 'A';
            Y = int.Parse(input.Substring(1)) - 1;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;

            Point other = (Point)obj;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() * 7) ^ Y.GetHashCode();
        }
    }
}
