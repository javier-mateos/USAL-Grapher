using System.ComponentModel;
using System.Windows.Input;

namespace Grapher
{
    class EditGraphViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Graph GraphObject { get; set; }

        public ICommand ManualAdd { get; set; }
        public ICommand AutoAdd { get; set; }

        public EditGraphViewModel(Graph graph)
        {
            GraphObject = graph;

            ManualAdd = new RelayCommand<object>(ManualAddExecute, ManualAddCanExecute);
            AutoAdd = new RelayCommand<object>(AutoAddExecute, AutoAddCanExecute);

            EditGraphWindow wnd = new EditGraphWindow();
            wnd.DataContext = this;
            wnd.Show();
        }

        private bool ManualAddCanExecute(object obj)
        {
            return true;
        }

        private void ManualAddExecute(object obj)
        {
            ManualAddViewModel manualVM = new ManualAddViewModel(GraphObject.Points);
        }

        private bool AutoAddCanExecute(object obj)
        {
            return true;
        }

        private void AutoAddExecute(object obj)
        {

        }
    }
}
