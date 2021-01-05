using System.Windows;

namespace Grapher.Views
{
    /// <summary>
    /// Lógica de interacción para AddGraphWindow.xaml
    /// </summary>
    public partial class AddGraphWindow : Window
    {
        public AddGraphWindow()
        {
            InitializeComponent();

            this.DataContext = new AddGraphWindowViewModel();
        }
    }
}
