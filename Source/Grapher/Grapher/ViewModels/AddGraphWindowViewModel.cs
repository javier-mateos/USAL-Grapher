using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Grapher
{
    class AddGraphWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        String NewName { get; set; }

        public ICommand AddAction { get; set; }
        public ICommand CancelAction { get; set; }

        public AddGraphWindowViewModel()
        {
            AddAction = new RelayCommand<object>(AddActionActionExecute, AddActionCanExecute);
            CancelAction = new RelayCommand<object>(CancelActionExecute, CancelActionCanExecute);
        }

        private bool AddActionCanExecute(object obj)
        {
            return (NewName.Length != 0) ? true : false;
        }

        private void AddActionActionExecute(object obj)
        {
            
        }

        private bool CancelActionCanExecute(object obj)
        {
            return true;
        }

        private void CancelActionExecute(object obj)
        {

        }
    }
}
