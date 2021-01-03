using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Grapher
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Projects List
        /// </summary>
        public ObservableCollection<Project> Projects { get; set; }

        /// <summary>
        /// ViewModel Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            Projects = new ObservableCollection<Project>();

            Projects.Add(new Project { Name = "Test 1" });
            Projects.Add(new Project { Name = "Test 2" });
            Projects.Add(new Project { Name = "Test 3" });

            Projects[0].Graphs = new ObservableCollection<Graph>();
            Projects[0].Graphs.Add(new Graph { Name = "Test Graph 1" });

            Projects[1].Graphs = new ObservableCollection<Graph>();
            Projects[1].Graphs.Add(new Graph { Name = "Test Graph 1" });
            Projects[1].Graphs.Add(new Graph { Name = "Test Graph 2" });
            Projects[1].Graphs.Add(new Graph { Name = "Test Graph 3" });

            Projects[2].Graphs = new ObservableCollection<Graph>();
            Projects[2].Graphs.Add(new Graph { Name = "Test Graph 1" });
            Projects[2].Graphs.Add(new Graph { Name = "Test Graph 2" });
        }
    }
}
