using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Grapher
{
    class Project : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Project Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Projects Graphs
        /// </summary>
        public ObservableCollection<Graph> Graphs { get; set; }

        public Project()
        {
            Graphs = new ObservableCollection<Graph>();
        }
    }
}
