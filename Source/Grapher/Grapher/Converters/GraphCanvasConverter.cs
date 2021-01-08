using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grapher
{
    class GraphCanvasConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            ObservableCollection<Shape> CanvasDataTranslation = new ObservableCollection<Shape>();
            ObservableCollection<Graph> Graphs = (ObservableCollection<Graph>)value;
            Graph Graph = new Graph();

            for (int i = Graphs.Count-1; i >= 0; i--)
            { 
                Graph = Graphs[i];

                switch (Graph.Type)
                {
                    case GraphType.LineGraph:

                        Polyline newLine = new Polyline();

                        newLine.Visibility = (Graph.IsVisible) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                        newLine.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Graph.Color));
                        newLine.Opacity = Graph.Opacity;
                        newLine.StrokeThickness = Graph.Thickness;

                        foreach (Point2D point in Graph.Points)
                        {
                            newLine.Points.Add(new System.Windows.Point { X = point.XValue, Y = point.YValue });
                        }

                        CanvasDataTranslation.Add(newLine);

                        break;

                    case GraphType.BarGraph:



                        break;

                    default:
                        return CanvasDataTranslation;
                }
                
            }

            return CanvasDataTranslation;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
