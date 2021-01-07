using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Grapher
{
    class ManualAddViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Point2D NewPoint { get; set; }

        public ICommand Add { get; set; }
        public ICommand Cancel { get; set; }

        private ManualAddWindow Wnd { get; set; }
        private ObservableCollection<Point2D> localGraphPoints;

        public ManualAddViewModel(ObservableCollection<Point2D> graphPoints)
        {
            NewPoint = new Point2D();

            localGraphPoints = graphPoints;

            Add = new RelayCommand<object>(AddExecute, AddCanExecute);
            Cancel = new RelayCommand<object>(CancelExecute, CancelCanExecute);

            Wnd = new ManualAddWindow();
            Wnd.DataContext = this;

            Wnd.ShowDialog();
        }

        private bool AddCanExecute(object obj)
        {
            /* TODO: Data validation disable button */
            return true;
        }

        private void AddExecute(object obj)
        {
            localGraphPoints.Add(NewPoint);

            Wnd.DialogResult = true;

            return;
        }

        private bool CancelCanExecute(object obj)
        {
            return true;
        }

        private void CancelExecute(object obj)
        {
            Wnd.DialogResult = false;

            return;
        }
    }
}
