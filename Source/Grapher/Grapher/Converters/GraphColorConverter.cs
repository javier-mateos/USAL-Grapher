using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Grapher
{
    class GraphColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString((string)value)};
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
