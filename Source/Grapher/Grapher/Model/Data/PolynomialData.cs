using System.ComponentModel;


namespace Grapher
{
    class PolynomialData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Grade { get; set; }
        public int Value { get; set; }
    }
}
