using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Common.PathFinding
{
    public struct PositionCoord
    {
        public int X;
        public int Y;
        public PositionCoord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(PositionCoord input1, PositionCoord input2)
        {
            return input1.X == input2.X && input1.Y == input2.Y;
        }

        public static bool operator !=(PositionCoord input1, PositionCoord input2)
        {
            return !(input1==input2);
        }

        public static PositionCoord operator +(PositionCoord input1, PositionCoord input2)
        {
            return new PositionCoord() { X = input1.X + input2.X, Y = input1.Y + input2.Y };
        }
    }
}
