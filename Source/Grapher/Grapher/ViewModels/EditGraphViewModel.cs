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

        public ICommand EditPoint { get; set; }
        public ICommand DeletePoint { get; set; }
        public ICommand MoveUpPoint { get; set; }
        public ICommand MoveDownPoint { get; set; }

        public EditGraphViewModel(Graph graph)
        {
            GraphObject = graph;

            ManualAdd = new RelayCommand<object>(ManualAddExecute, ManualAddCanExecute);
            AutoAdd = new RelayCommand<object>(AutoAddExecute, AutoAddCanExecute);

            EditPoint = new RelayCommand<object>(EditPointExecute, EditPointCanExecute);
            DeletePoint = new RelayCommand<object>(DeletePointExecute, DeletePointCanExecute);
            MoveUpPoint = new RelayCommand<object>(MoveUpPointExecute, MoveUpPointCanExecute);
            MoveDownPoint = new RelayCommand<object>(MoveDownPointExecute, MoveDownPointCanExecute);

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
            AutoAddViewModel autoVM = new AutoAddViewModel(GraphObject.PolyExp);
        }

        private bool EditPointCanExecute(object obj)
        {
            return (((IList)obj).Count > 0) ? true : false;
        }

        private void EditPointExecute(object obj)
        {
            if (obj != null)
            {
                /* SelectedItems Casting */
                IList items = (IList)obj;
                var selection = items.Cast<Point2D>();

                /* SelectedData List */
                List<Point2D> selectedData = new List<Point2D>();

                foreach (Point2D point in selection)
                {
                    selectedData.Add(point);
                }

                foreach (Point2D point in selectedData)
                {
                    EditPointViewModel editVM = new EditPointViewModel(point);
                }
            }
        }

        private bool DeletePointCanExecute(object obj)
        {
            return (((IList)obj).Count > 0) ? true : false;
        }

        private void DeletePointExecute(object obj)
        {
            if (obj != null)
            { 
                /* SelectedItems Casting */
                IList items = (IList)obj;
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

        private bool MoveUpPointCanExecute(object obj)
        {
            return (((IList)obj).Count > 0) ? true : false;
        }

        private void MoveUpPointExecute(object obj)
        {
            if (obj != null)
            {
                /* SelectedItems Casting */
                IList items = (IList)obj;
                var selection = items.Cast<Point2D>();

                /* SelectedData List */
                List<Point2D> selectedData = new List<Point2D>();

                foreach (Point2D point in selection)
                {
                    selectedData.Add(point);
                }

                foreach (Point2D point in selectedData)
                {
                    int pointIndex = GraphObject.Points.IndexOf(point);

                    if (pointIndex == 0)
                        continue;

                    Point2D temp = GraphObject.Points[pointIndex];

                    GraphObject.Points[pointIndex] = GraphObject.Points[pointIndex - 1];
                    GraphObject.Points[pointIndex - 1] = temp;
                }
            }
        }

        private bool MoveDownPointCanExecute(object obj)
        {
            return (((IList)obj).Count > 0) ? true : false;
        }

        private void MoveDownPointExecute(object obj)
        {
            if (obj != null)
            {
                /* SelectedItems Casting */
                IList items = (IList)obj;
                var selection = items.Cast<Point2D>();

                /* SelectedData List */
                List<Point2D> selectedData = new List<Point2D>();

                foreach (Point2D point in selection)
                {
                    selectedData.Add(point);
                }

                foreach (Point2D point in selectedData)
                {
                    int pointIndex = GraphObject.Points.IndexOf(point);

                    if (pointIndex == GraphObject.Points.Count-1)
                        continue;

                    Point2D temp = GraphObject.Points[pointIndex];

                    GraphObject.Points[pointIndex] = GraphObject.Points[pointIndex + 1];
                    GraphObject.Points[pointIndex + 1] = temp;
                }
            }
        }
    }
}
