using System.ComponentModel;


namespace Grapher
{
    class EditGraphViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Graph GraphObject { get; set; }

        public EditGraphViewModel(Graph graph)
        {
            GraphObject = graph;

            EditGraphWindow wnd = new EditGraphWindow();
            wnd.DataContext = this;
            wnd.Show();
        }
    }
}
