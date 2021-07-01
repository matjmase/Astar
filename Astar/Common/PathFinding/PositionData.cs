using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Common.PathFinding
{
    public struct PositionData
    {
        public PositionCoord Coord;
        public double DistanceFromDestination;
        public double DistanceFromStart;
        public PositionData(int x, int y, double distanceFromDestination, double distanceFromStart)
        {
            Coord = new PositionCoord() { X = x, Y = y };
            DistanceFromDestination = distanceFromDestination;
            DistanceFromStart = distanceFromStart;
        }
    }
}
