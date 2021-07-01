using Astar.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Astar.Converters
{
    public class TileStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var enumState = (TileState)value;
                var parameterColor = (string[])parameter;
                try
                {
                    return (Brush)typeof(Brushes).GetProperty(parameterColor[(int)enumState]).GetValue(null, null);
                }
                catch (Exception e)
                {
                    return Brushes.Black;
                }
            }
            catch (Exception e)
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
