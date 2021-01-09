using System.ComponentModel;
using System.Windows.Input;

namespace Grapher
{
    class AutoAddViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private AutoAddWindow Wnd { get; set; }

        #region Public Members
        public Graph GraphObject { get; set; }
        #endregion

        #region Commands
        public ICommand Add { get; set; }
        public ICommand Cancel { get; set; }
        #endregion

        public AutoAddViewModel(PolynomialExpression poly)
        {
            Add = new RelayCommand<object>(AddExecute, AddCanExecute);
            Cancel = new RelayCommand<object>(CancelExecute, CancelCanExecute);

            Wnd = new AutoAddWindow();
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
