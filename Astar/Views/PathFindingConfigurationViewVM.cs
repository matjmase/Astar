using Astar.Common;
using Astar.Common.PathFinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Views
{
    public class PathFindingConfigurationViewVM : INotifyPropertyChanged
    {
        private TraversalOrientation _pathingMode = TraversalOrientation.Orthogonal;
        private double _startDistanceCoef = 1;
        private double _absoluteDistanceCoef = 1;
        private double _gradTickInSeconds = 0.2;

        public TraversalOrientation PathingMode
        {
            get { return _pathingMode; }
            set
            {
                if (value != _pathingMode)
                {
                    _pathingMode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double StartDistanceCoef
        {
            get
            {
                return _startDistanceCoef;
            }
            set
            {
                if (value != _startDistanceCoef)
                {
                    _startDistanceCoef = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double AbsoluteDistanceCoef
        {
            get { return _absoluteDistanceCoef; }
            set
            {
                if(value != _absoluteDistanceCoef)
                {
                    _absoluteDistanceCoef = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double GradTickInSeconds
        {
            get { return _gradTickInSeconds; }
            set
            {
                if (value != _gradTickInSeconds)
                {
                    _gradTickInSeconds = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DelegateCommand TogglePathingModeCommand => new DelegateCommand((obj) =>
        {
            var enumVals = Enum.GetValues(typeof(TraversalOrientation)).Cast<TraversalOrientation>().ToList();

            var curIndex = enumVals.IndexOf(PathingMode);
            var nextIndex = (curIndex + 1) % enumVals.Count;

            PathingMode = enumVals[nextIndex];
        });

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
