using System;
using System.Globalization;
using System.Windows.Data;

namespace Grapher
{
    class GraphTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri newImage;

            switch((GraphType)value)
            {
                case GraphType.LineGraph:
                    newImage = new Uri("/Images/LineChartGraph.png", UriKind.Relative);
                    break;
                case GraphType.BarGraph:
                    newImage = new Uri("/Images/BarChartGraph.png", UriKind.Relative);
                    break;
                default:
                    newImage = new Uri("/Images/DummyImage.png", UriKind.Relative);
                    break;
            }

            return newImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
