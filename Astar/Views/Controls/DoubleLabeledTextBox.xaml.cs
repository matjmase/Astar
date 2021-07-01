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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Astar.Views.Controls
{
    /// <summary>
    /// Interaction logic for DoubleLabeledTextBox.xaml
    /// </summary>
    public partial class DoubleLabeledTextBox : UserControl
    {
        public static readonly DependencyProperty LabelValueProperty =
            DependencyProperty.Register(
            "LabelValue", typeof(string),
            typeof(DoubleLabeledTextBox)
            );
        public string LabelValue
        {
            get { return (string)GetValue(LabelValueProperty); }
            set { SetValue(LabelValueProperty, value); }
        }

        public static readonly DependencyProperty DoubleValueProperty =
            DependencyProperty.Register(
            "DoubleValue", typeof(double),
            typeof(DoubleLabeledTextBox)
            );

        public double DoubleValue
        {
            get { return (double)GetValue(DoubleValueProperty); }
            set { SetValue(DoubleValueProperty, value); }
        }

        public static readonly DependencyProperty LabelFontSizeProperty =
            DependencyProperty.Register(
            "LabelFontSize", typeof(double),
            typeof(DoubleLabeledTextBox)
            );
        public double LabelFontSize
        {
            get { return (double)GetValue(LabelFontSizeProperty); }
            set { SetValue(LabelFontSizeProperty, value); }
        }

        public static readonly DependencyProperty ValueFontSizeProperty =
            DependencyProperty.Register(
            "ValueFontSize", typeof(double),
            typeof(DoubleLabeledTextBox)
            );
        public double ValueFontSize
        {
            get { return (double)GetValue(ValueFontSizeProperty); }
            set { SetValue(ValueFontSizeProperty, value); }
        }

        public DoubleLabeledTextBox()
        {
            InitializeComponent();
        }
    }
}
