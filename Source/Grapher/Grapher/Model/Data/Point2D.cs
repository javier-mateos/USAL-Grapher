using System.ComponentModel;

namespace Grapher
{
    class Point2D : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double XValue { get; set; }
        public double YValue { get; set; }
    }
}
