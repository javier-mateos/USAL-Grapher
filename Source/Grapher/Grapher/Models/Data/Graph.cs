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
    }
}
