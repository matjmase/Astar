using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Common.PathFinding
{
    public class Astar2d<T>
    {
        private readonly PositionCoord[] _orthogonalDirections = new PositionCoord[]
        {
            new PositionCoord(0,1),
            new PositionCoord(0,-1),
            new PositionCoord(-1,0),
            new PositionCoord(1,0),
        };

        private readonly PositionCoord[] _diagonalDirections = new PositionCoord[]
        {
            new PositionCoord(1,1),
            new PositionCoord(-1,-1),
            new PositionCoord(-1,1),
            new PositionCoord(1,-1),
        };

        private readonly Tuple<PositionCoord, PositionCoord[]>[] _diagonalDirectionsConstraint = new Tuple<PositionCoord, PositionCoord[]>[]
        {
            new Tuple<PositionCoord, PositionCoord[]>(new PositionCoord(1,1), new PositionCoord[]{ new PositionCoord(1,0), new PositionCoord(0,1) }),
            new Tuple<PositionCoord, PositionCoord[]>(new PositionCoord(-1,1), new PositionCoord[]{ new PositionCoord(-1,0), new PositionCoord(0,1) }),
            new Tuple<PositionCoord, PositionCoord[]>(new PositionCoord(1,-1), new PositionCoord[]{ new PositionCoord(1,0), new PositionCoord(0,-1) }),
            new Tuple<PositionCoord, PositionCoord[]>(new PositionCoord(-1,-1), new PositionCoord[]{ new PositionCoord(-1,0), new PositionCoord(0,-1) }),
        };

        private Astar2dModel<T> _model;

        public delegate void PointTraversed(PositionData newNeighbor);
        public event PointTraversed GradientPointFound;
        public event PointTraversed PathPointFound;

        public Astar2d(T[,] scene, double startDistCoef, double absStopDist, Func<T, bool> isObstacle)
        {
            _model = new Astar2dModel<T>() { Scene = scene, StartDistCoef = startDistCoef, AbsStopDistCoef = absStopDist, IsObstacle = isObstacle };
        }

        #region publicFunctions
        
        public Astar2dResults CalcPath(Func<T, bool> isStart, Func<T, bool> isStop, TraversalOrientation orientation)
        {
            var playerCoord = ScanForFirstValue(_model.Scene, isStart);
            var destCoord = ScanForFirstValue(_model.Scene, isStop);
            if (playerCoord == null || destCoord == null)
                throw new ArgumentException("Start or Stop could not be found");

            return CalcPath((PositionCoord)playerCoord, (PositionCoord)destCoord, orientation);
        }

        public Astar2dResults CalcPath(PositionCoord start, PositionCoord stop, TraversalOrientation orientation)
        {
            var gradient = CalcGradient(start, stop, orientation);
            var path = FindPathFromGradient(gradient);

            return new Astar2dResults() { Gradient = gradient, Path = path.ToList(), TotalDistance = gradient[_model.Destination.X, _model.Destination.Y] };
        }

        #endregion

        #region CalculateGradient

        private double[,] CalcGradient(PositionCoord start, PositionCoord stop, TraversalOrientation orientation)
        {
            _model.Start = start;
            _model.Destination = stop;
            _model.Orientation = orientation;

            var gradient = new double[_model.Scene.GetLength(0), _model.Scene.GetLength(1)];

            //init
            for (var i = 0; i < gradient.GetLength(0); i++)
            {
                for (var j = 0; j < gradient.GetLength(1); j++)
                {
                    //untraversed value
                    gradient[i, j] = -1;
                }
            }

            var neightbors = new Heap<PositionData>((first, second) => first.DistanceFromStart * _model.StartDistCoef + first.DistanceFromDestination * _model.AbsStopDistCoef < second.DistanceFromStart * _model.StartDistCoef + second.DistanceFromDestination * _model.AbsStopDistCoef);
            neightbors.Add(new PositionData(_model.Start.X, _model.Start.Y, CalcDist(_model.Start, _model.Destination), 0));
            gradient[_model.Start.X, _model.Start.Y] = 0;

            bool destinationFound = false;
            while (!neightbors.IsEmpty() && !destinationFound)
            {
                var focus = neightbors.Pop();
                var newNeighbors = FindNeighbors(gradient, focus.Coord, NeighborAquisitionMethod.Untraversed);

                foreach (var newNeighbor in newNeighbors)
                {
                    var neighborPosData = new PositionData() { Coord = newNeighbor, DistanceFromStart = focus.DistanceFromStart + CalcDist(focus.Coord, newNeighbor), DistanceFromDestination = CalcDist(newNeighbor, _model.Destination) };
                    
                    //populate heap
                    neightbors.Add(neighborPosData);
                    //fill in gradient
                    gradient[neighborPosData.Coord.X, neighborPosData.Coord.Y] = neighborPosData.DistanceFromStart;

                    GradientPointFound?.Invoke(neighborPosData);

                    if (newNeighbor == _model.Destination)
                        destinationFound = true;
                }
            }

            if (!destinationFound)
            {
                throw new ArgumentOutOfRangeException("Game is unsolvable");
            }

            return gradient;
        }

        #endregion

        #region PathFromGradient

        private IEnumerable<PositionCoord> FindPathFromGradient(double[,] gradient)
        {
            var path = new LinkedList<PositionCoord>();
            path.AddLast(_model.Destination);
            PathPointFound?.Invoke(new PositionData() { Coord = path.Last(), DistanceFromStart = gradient[path.Last().X, path.Last().Y], DistanceFromDestination = CalcDist(path.Last(), _model.Destination) });
            while (path.Last() != _model.Start)
            {
                path.AddLast(FindNextLoc(gradient, path.Last()));
                PathPointFound?.Invoke(new PositionData() { Coord = path.Last(), DistanceFromStart = gradient[path.Last().X, path.Last().Y], DistanceFromDestination = CalcDist(path.Last(), _model.Destination) });
            }

            //start to dest
            path.Reverse();

            return path;
        }

        private PositionCoord FindNextLoc(double[,] gradient, PositionCoord curPos)
        {
            var adjacentSpots = FindNeighbors(gradient, curPos, NeighborAquisitionMethod.Traversed);

            var applicable = new List<Tuple<PositionCoord, double>>();

            foreach (var adjacentSpot in adjacentSpots)
            {
                applicable.Add(new Tuple<PositionCoord, double>(adjacentSpot, gradient[adjacentSpot.X, adjacentSpot.Y] + CalcDist(adjacentSpot, curPos)));
            }

            if (applicable.Count == 0)
                throw new ArgumentException("Path is not acheivable with current gradient");

            var min = applicable.OrderBy(e => e.Item2).First();

            return min.Item1;
        }

        #endregion

        #region NeighborAquisition

        private enum NeighborAquisitionMethod
        {
            Untraversed,
            Traversed
        }

        private IEnumerable<PositionCoord> FindNeighbors(double[,] gradient, PositionCoord focus, NeighborAquisitionMethod aquisitionMethod)
        {
            var allNeighbors = new LinkedList<PositionCoord>();

            //all need orthogonal
            for (var i = 0; i < _orthogonalDirections.Length; i++)
            {
                allNeighbors.AddLast(focus + _orthogonalDirections[i]);
            }

            switch (_model.Orientation)
            {
                case TraversalOrientation.Orthogonal:
                    break;
                case TraversalOrientation.OrthoDiag:
                    for (var i = 0; i < _diagonalDirections.Length; i++)
                    {
                        allNeighbors.AddLast(focus + _diagonalDirections[i]);
                    }
                    break;
                case TraversalOrientation.OrthoDiagNoCorners:
                    for (var i = 0; i < _diagonalDirectionsConstraint.Length; i++)
                    {
                        if (!_diagonalDirectionsConstraint[i].Item2.All(e => InBoundsNotObstacle(focus + e)))
                            continue;

                        allNeighbors.AddLast(focus + _diagonalDirectionsConstraint[i].Item1);
                    }
                    break;
                default:
                    throw new NotImplementedException("New Traversal orientation that is not implemented");
            }

            switch (aquisitionMethod)
            {
                case NeighborAquisitionMethod.Untraversed:
                    return allNeighbors.Where(e => InBoundsNotObstacleUntraversed(gradient, e));
                case NeighborAquisitionMethod.Traversed:
                    return allNeighbors.Where(e => InBoundsAndTraversed(gradient, e));
                default:
                    return allNeighbors;
            }

        }

        #endregion

        #region LogicFunctions

        private bool InBoundsAndTraversed(double[,] gradient, PositionCoord location)
        {
            return InBounds(location) && gradient[location.X, location.Y] != -1;
        }

        private bool InBoundsNotObstacleUntraversed(double[,] gradient, PositionCoord position)
        {
            return InBoundsNotObstacle(position) && gradient[position.X, position.Y] == -1;
        }

        private bool InBoundsNotObstacle(PositionCoord position)
        {
            return InBounds(position) && !_model.IsObstacle(_model.Scene[position.X, position.Y]);
        }

        private bool InBounds(PositionCoord position)
        {
            //Not out of bounds
            return !(position.X < 0 || position.X >= _model.Scene.GetLength(0) || position.Y < 0 || position.Y >= _model.Scene.GetLength(1));
        }

        #endregion

        #region Misc

        private PositionCoord? ScanForFirstValue(T[,] arr, Func<T, bool> isObjectType)
        {
            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    if (isObjectType(arr[i, j]))
                        return new PositionCoord(i, j);
                }
            }
            return null;
        }

        private double CalcDist(PositionCoord from, PositionCoord to)
        {
            return Math.Sqrt(Math.Pow(from.X - to.X, 2) + Math.Pow(from.Y - to.Y, 2));
        }

        #endregion
    }
}
