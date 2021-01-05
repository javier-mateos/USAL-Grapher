using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Grapher
{
    class Graph : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Graph Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Graph Type
        /// </summary>
        public GraphType Type { get; set; } = GraphType.LineGraph;

        /// <summary>
        /// Graph Visibility
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Graph Points Data
        /// </summary>
        public ObservableCollection<Point2D> Points {get; set;}

        /// <summary>
        /// Graph Constructor
        /// </summary>
        public Graph()
        {
            Points = new ObservableCollection<Point2D>();

            Points.Add(new Point2D { XValue = 15, YValue = 20 });
            Points.Add(new Point2D { XValue = 20, YValue = 30 });
            Points.Add(new Point2D { XValue = 40, YValue = 60 });
            Points.Add(new Point2D { XValue = 50, YValue = 10 });
        }
    }
}
