using Astar.Common;
using Astar.Models;
using Astar.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Astar.Views
{
    public class GridControlViewVM : INotifyPropertyChanged
    {
        private TileState _tileDrawingMode = TileState.Open;

        public TileState TileDrawingMode
        {
            get { return _tileDrawingMode; }
            set
            {
                if (value != _tileDrawingMode)
                {
                    _tileDrawingMode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event Action ClearBoard;
        public event Action<BoardDimensions> SetNewBoardSize;

        public DelegateCommand NewSizeCommand => new DelegateCommand((obj) => SetNewBoardSize?.Invoke(GetNewSize()));
        public DelegateCommand ClearBoardCommand => new DelegateCommand((obj) => ClearBoard?.Invoke());
        public DelegateCommand AboutAppCommand => new DelegateCommand((obj) => new AboutWindow().ShowDialog());

        public DelegateCommand ToggleTileDrawingModeCommand => new DelegateCommand((obj) =>
        {
            var enums = Enum.GetValues(typeof(TileState)).Cast<TileState>().ToList();
            enums.Remove(TileState.Path);
            enums.Remove(TileState.Traversed);

            var index = enums.IndexOf(TileDrawingMode);
            var nextIndex = (index + 1) % enums.Count;

            TileDrawingMode = enums[nextIndex];
        });

        public BoardDimensions GetNewSize()
        {
            var configWindow = new ConfigSizeWindow();

            configWindow.ShowDialog();
            if (configWindow.AcceptedConfirm)
            {
                return new BoardDimensions() { Rows = configWindow.IntRows, Columns = configWindow.IntColumns };
            }
            else
            {
                App.Current.Shutdown();
                return new BoardDimensions();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
