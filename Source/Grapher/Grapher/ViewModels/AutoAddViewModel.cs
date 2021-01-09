using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Grapher
{
    class AutoAddViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Private Members

        private AutoAddWindow Wnd { get; set; }

        #endregion


        #region Public Members

        public PolynomialExpression PolyExp { get; set; }
        public string StringExpr { get; set; }

        #endregion


        #region Commands

        public ICommand Add { get; set; }
        public ICommand Cancel { get; set; }

        #endregion

        public AutoAddViewModel(PolynomialExpression poly)
        {
            /* Poly */
            PolyExp = poly;

            /* Commands */
            Add = new RelayCommand<object>(AddExecute, AddCanExecute);
            Cancel = new RelayCommand<object>(CancelExecute, CancelCanExecute);

            /* Window*/
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
