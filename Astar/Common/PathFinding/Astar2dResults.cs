using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Common.PathFinding
{
    public class Astar2dResults
    {
        public double[,] Gradient;
        public List<PositionCoord> Path;
        public double TotalDistance;
    }
}
