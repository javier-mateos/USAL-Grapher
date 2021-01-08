using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grapher
{
    class GraphCanvasConverter : IValueConverter, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<Shape> CanvasDataTranslation { get; set; } = new ObservableCollection<Shape>();
        ObservableCollection<Graph> Graphs { get; set; } = new ObservableCollection<Graph>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            Graphs = (ObservableCollection<Graph>)value;


            foreach (Graph graph in Graphs)
            {
                switch (graph.Type)
                {
                    case GraphType.LineGraph:

                        Polyline newLine = new Polyline();

                        newLine.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(graph.Color));
                        newLine.Opacity = graph.Opacity;
                        newLine.StrokeThickness = graph.Thickness;

                        foreach (Point2D point in graph.Points)
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
