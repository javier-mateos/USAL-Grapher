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


        public AutoAddViewModel(PolynomialExpression poly)
        {
            /* Poly */
            PolyExp = poly;

            /* Window*/
            Wnd = new AutoAddWindow();
            Wnd.DataContext = this;

            Wnd.Show();
        }
    }
}
