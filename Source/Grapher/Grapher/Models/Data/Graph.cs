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
    }
}
