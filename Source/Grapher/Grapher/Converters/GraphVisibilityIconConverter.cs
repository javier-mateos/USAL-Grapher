using System;
using System.Globalization;
using System.Windows.Data;

namespace Grapher
{
    class GraphVisibilityIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri newImage;

            if ((bool)value)   
                newImage = new Uri("/Images/VisibilityOn.png", UriKind.Relative);
            else
                newImage = new Uri("/Images/VisibilityOff.png", UriKind.Relative);

            return newImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
