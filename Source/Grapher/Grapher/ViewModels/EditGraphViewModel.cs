using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Grapher
{
    class EditGraphViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Graph GraphObject { get; set; }

        public ICommand ManualAdd { get; set; }
        public ICommand AutoAdd { get; set; }
        public ICommand DeleteData { get; set; }

        public EditGraphViewModel(Graph graph)
        {
            GraphObject = graph;

            ManualAdd = new RelayCommand<object>(ManualAddExecute, ManualAddCanExecute);
            AutoAdd = new RelayCommand<object>(AutoAddExecute, AutoAddCanExecute);
            DeleteData = new RelayCommand<object>(DeleteDataExecute, DeleteDataCanExecute);

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

        private bool DeleteDataCanExecute(object obj)
        {
            return (((IList)obj).Count > 0) ? true : false;
        }

        private void DeleteDataExecute(object obj)
        {
            if (obj != null) { 

                /* SelectedItems Casting */
                System.Collections.IList items = (System.Collections.IList)obj;
                var selection = items.Cast<Point2D>();

                /* SelectedData List */
                List<Point2D> selectedData = new List<Point2D>();

                foreach (Point2D point in selection)
                {
                    selectedData.Add(point);
                }

                foreach (Point2D point in selectedData)
                {
                    GraphObject.Points.Remove(point);
                }
            }
        }
    }
}
