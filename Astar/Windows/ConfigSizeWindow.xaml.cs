using Astar.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Astar.Windows
{
    /// <summary>
    /// Interaction logic for ConfigSizeWindow.xaml
    /// </summary>
    public partial class ConfigSizeWindow : Window
    {
        public static readonly DependencyProperty IntRowsProperty =
            DependencyProperty.Register(
            "IntRows", typeof(int),
            typeof(ConfigSizeWindow),
            new PropertyMetadata(1)
            );
        public int IntRows
        {
            get { return (int)GetValue(IntRowsProperty); }
            set { SetValue(IntRowsProperty, value); }
        }

        public static readonly DependencyProperty IntColumnsProperty =
            DependencyProperty.Register(
            "IntColumns", typeof(int),
            typeof(ConfigSizeWindow),
            new PropertyMetadata(1)
            );
        public int IntColumns
        {
            get { return (int)GetValue(IntColumnsProperty); }
            set { SetValue(IntColumnsProperty, value); }
        }

        public static readonly DependencyProperty AcceptedConfirmProperty =
            DependencyProperty.Register(
            "AcceptedConfirm", typeof(bool),
            typeof(ConfigSizeWindow)
            );
        public bool AcceptedConfirm
        {
            get { return (bool)GetValue(AcceptedConfirmProperty); }
            set { SetValue(AcceptedConfirmProperty, value); }
        }

        public static readonly DependencyProperty AcceptCommandProperty =
            DependencyProperty.Register(
            "AcceptCommand", typeof(ICommand),
            typeof(ConfigSizeWindow)
            );
        public ICommand AcceptCommand
        {
            get { return (ICommand)GetValue(AcceptCommandProperty); }
            set { SetValue(AcceptCommandProperty, value); }
        }

        public ConfigSizeWindow()
        {
            AcceptCommand = new DelegateCommand(obj => Confirmation());
            InitializeComponent();
        }

        private void Confirmation()
        {
            AcceptedConfirm = true;
            this.Close();
        }

    }
}
