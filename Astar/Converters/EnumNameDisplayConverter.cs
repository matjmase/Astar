﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Astar.Converters
{
    public class EnumNameDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is Type && ((Type)parameter).IsEnum && value.GetType() == (Type) parameter)
            {
                return Enum.GetName((Type)parameter, value);
            }
            else
            {
                throw new ArgumentException("Parameter must be Type of enum and value must be a value of that enum");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
