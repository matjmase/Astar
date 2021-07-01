using Astar.Common;
using Astar.Common.PathFinding;
using Astar.Models;
using Astar.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Astar
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private GridControlViewVM _gridControl = new GridControlViewVM();
        private PathFindingConfigurationViewVM _pathConfig = new PathFindingConfigurationViewVM();
        private bool _gradGenerationInProgress = false;
        private string _totalResultantDistance = "---";

        public GridControlViewVM GridControl
        {
            get
            {
                return _gridControl;
            }
            set
            {
                _gridControl = value;
                NotifyPropertyChanged();
            }
        }

        public PathFindingConfigurationViewVM PathConfig
        {
            get
            {
                return _pathConfig;
            }
            set
            {
                _pathConfig = value;
                NotifyPropertyChanged();
            }
        }
        public bool GradGenerationInProgress
        {
            get { return _gradGenerationInProgress; }
            set
            {
                if (value != _gradGenerationInProgress)
                {
                    _gradGenerationInProgress = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string TotalResultantDistance
        {
            get { return _totalResultantDistance; }
            set
            {
                if (value != _totalResultantDistance)
                {
                    _totalResultantDistance = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<ObservableCollection<MultiStateTile>> Tiles { get; set; } = new ObservableCollection<ObservableCollection<MultiStateTile>>();

        public DelegateCommand WindowIsLoaded => new DelegateCommand((obj) =>
        {
            var initSize = GridControl.GetNewSize();
            InitializeNewBoard(initSize);
        });

        public DelegateCommand CalculatePathCommand => new DelegateCommand((obj) => CalculatePath());
        public DelegateCommand AnimateGradientCommand => new DelegateCommand((obj) => AnimateGradient());

        public MainWindowVM()
        {
            GridControl.SetNewBoardSize += InitializeNewBoard;
            GridControl.ClearBoard += ClearBoard;

        }

        ~MainWindowVM()
        {
            GridControl.SetNewBoardSize -= InitializeNewBoard;
            GridControl.ClearBoard -= ClearBoard;
        }

        private void InitializeNewBoard(BoardDimensions dims)
        {
            Tiles.Clear();

            for (var i = 0; i < dims.Rows; i++)
            {
                Tiles.Add(new ObservableCollection<MultiStateTile>());
                for (var j = 0; j < dims.Columns; j++)
                {
                    Tiles.Last().Add(new MultiStateTile(TileState.Open, ClearStarts, ClearStops, ClearGradAndPath, () => GridControl.TileDrawingMode, () => GradGenerationInProgress));
                }
            }
        }

        #region MultiStateTileMethods

        private void ClearStarts()
        {
            ClearState(TileState.Start);
        }
        private void ClearStops()
        {

            ClearState(TileState.Stop);
        }
        private void ClearGradAndPath()
        {
            TotalResultantDistance = "---";
            ClearState(TileState.Traversed);
            ClearState(TileState.Path);
        }

        private void ClearState(TileState targetState)
        {
            //can be optimize by keeping up with starts, stops, grad, and path. 
            //this is for simplicity sake in a toy app.
            for (var i = 0; i < Tiles.Count; i++)
            {
                for (var j = 0; j < Tiles[i].Count; j++)
                {
                    if (targetState == Tiles[i][j].State)
                        Tiles[i][j].State = TileState.Open;
                }
            }
        }

        private void ClearBoard()
        {
            for (var i = 0; i < Tiles.Count; i++)
            {
                for (var j = 0; j < Tiles[i].Count; j++)
                {
                    Tiles[i][j].State = TileState.Open;
                }
            }
        }

        #endregion

        #region Pathfinding

        private void CalculatePath()
        {
            ClearGradAndPath();
            var grid = Generate2dArray();

            try
            {
                var astar = new Astar2d<MultiStateTile>(grid, PathConfig.StartDistanceCoef, PathConfig.AbsoluteDistanceCoef, e => e.State == TileState.Obstacle);
                var results = astar.CalcPath(e => e.State == TileState.Start, e => e.State == TileState.Stop, PathConfig.PathingMode);

                foreach (var point in results.Path)
                {
                    if (Tiles[point.X][point.Y].State == TileState.Start || Tiles[point.X][point.Y].State == TileState.Stop)
                        continue;

                    Tiles[point.X][point.Y].State = TileState.Path;
                }

                TotalResultantDistance = results.TotalDistance.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void AnimateGradient()
        {
            if (GradGenerationInProgress)
            {
                GradGenerationInProgress = false;
                return;
            }    

            ClearGradAndPath();
            var grid = Generate2dArray();
            var astar = new Astar2d<MultiStateTile>(grid, PathConfig.StartDistanceCoef, PathConfig.AbsoluteDistanceCoef, e => e.State == TileState.Obstacle);

            await Task.Run(() =>
            {
                try
                {

                    GradGenerationInProgress = true;
                    astar.GradientPointFound += Astar_GradientPointFound;
                    astar.PathPointFound += Astar_PathPointFound;
                    var results = astar.CalcPath(e => e.State == TileState.Start, e => e.State == TileState.Stop, PathConfig.PathingMode);

                    TotalResultantDistance = results.TotalDistance.ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    GradGenerationInProgress = false;
                    astar.GradientPointFound -= Astar_GradientPointFound;
                    astar.PathPointFound -= Astar_PathPointFound;
                }
            });
        }

        private void Astar_PathPointFound(PositionData newNeighbor)
        {
            SetStateAndSleep(newNeighbor, TileState.Path);
        }

        private void Astar_GradientPointFound(PositionData newNeighbor)
        {
            SetStateAndSleep(newNeighbor, TileState.Traversed);
        }

        private void SetStateAndSleep(PositionData point, TileState newState)
        {
            if (Tiles[point.Coord.X][point.Coord.Y].State == TileState.Start || Tiles[point.Coord.X][point.Coord.Y].State == TileState.Stop)
                return;

            Tiles[point.Coord.X][point.Coord.Y].State = newState;
            if (GradGenerationInProgress)
                Thread.Sleep((int)(PathConfig.GradTickInSeconds * 1000));
        }

        private MultiStateTile[,] Generate2dArray()
        {
            var grid = new MultiStateTile[Tiles.Count, Tiles[0].Count];

            for (var i = 0; i < Tiles.Count; i++)
            {
                for (var j = 0; j < Tiles[0].Count; j++)
                {
                    grid[i, j] = Tiles[i][j];
                }
            }

            return grid;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
