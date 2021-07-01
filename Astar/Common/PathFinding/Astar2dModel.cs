using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Common.PathFinding
{
    public class Astar2dModel<T>
    {
        public T[,] Scene;
        public Func<T, bool> IsObstacle;
        public PositionCoord Start;
        public PositionCoord Destination;
        public double StartDistCoef;
        public double AbsStopDistCoef;
        public TraversalOrientation Orientation;
    }
}
