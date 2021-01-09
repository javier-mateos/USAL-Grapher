using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Grapher
{
    class PolynomialExpression : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<double> Values { get; set; }
        public double XMinVal { get; set; }
        public double XMaxVal { get; set; }

        public PolynomialExpression()
        {

        }
    }
}
