using Astar.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Astar.Models
{
    public class MultiStateTile : INotifyPropertyChanged
    {
        private TileState _state;

        public TileState State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Action _clearStarts;
        private Action _clearStops;
        private Action _clearGradAndPath;
        private Func<TileState> _tileMode;
        private Func<bool> _isReadonlyMode;

        public DelegateCommand MouseDownChange => new DelegateCommand((obj) =>
        {
            if (_isReadonlyMode())
                return;

            _clearGradAndPath();

            var args = (MouseButtonEventArgs)obj;
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                SetNewState(_tileMode());
            }
        });

        public DelegateCommand MouseMoveChange => new DelegateCommand((obj) =>
        {
            if (_isReadonlyMode())
                return;

            var args = (MouseEventArgs)obj;

            if (args.LeftButton == MouseButtonState.Pressed)
            {
                SetNewState(_tileMode());
            }
        });

        private void SetNewState(TileState currentMode)
        {
            switch (currentMode)
            {
                case TileState.Start:
                    _clearStarts();
                    break;
                case TileState.Stop:
                    _clearStops();
                    break;
            }
            State = currentMode;
        }

        public MultiStateTile(TileState initState, Action clearStarts, Action clearStops, Action clearGradAndPath, Func<TileState> tileMode, Func<bool> isReadonlyMode)
        {
            State = initState;
            _clearStarts = clearStarts;
            _clearStops = clearStops;
            _clearGradAndPath = clearGradAndPath;
            _tileMode = tileMode;
            _isReadonlyMode = isReadonlyMode;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum TileState : int
    {
        Open = 0,
        Obstacle = 1,
        Start = 2,
        Stop = 3,
        Path = 4,
        Traversed = 5
    }
}
