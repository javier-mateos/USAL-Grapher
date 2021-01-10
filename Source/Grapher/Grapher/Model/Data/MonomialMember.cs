using System.ComponentModel;


namespace Grapher
{
    class MonomialMember : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Grade { get; set; }
        public int Value { get; set; }
    }
}
