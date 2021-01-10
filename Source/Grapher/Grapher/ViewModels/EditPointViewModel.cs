using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Grapher
{
    class EditPointViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand Edit { get; set; }
        public ICommand Cancel { get; set; }

        public Point2D Point { get; set; }
        public Point2D EditedPoint { get; set; }

        private EditPointWindow Wnd { get; set; }

        public EditPointViewModel(Point2D point)
        {
            EditedPoint = new Point2D();

            EditedPoint.XValue = point.XValue;
            EditedPoint.YValue = point.YValue;

            Point = point;

            Edit = new RelayCommand<object>(EditExecute, EditCanExecute);
            Cancel = new RelayCommand<object>(CancelExecute, CancelCanExecute);

            Wnd = new EditPointWindow();
            Wnd.DataContext = this;

            Wnd.ShowDialog();
        }

        private bool EditCanExecute(object obj)
        {
            /* TODO: Data validation disable button */
            return true;
        }

        private void EditExecute(object obj)
        {
            Point.XValue = EditedPoint.XValue;
            Point.YValue = EditedPoint.YValue;

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
