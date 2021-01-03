using System.Windows;
using System.Windows.Controls;

namespace Grapher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();
        }

        private void swapPanelsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Dock tmpDock = DockPanel.GetDock(this.toolBar);

            DockPanel.SetDock(this.toolBar, DockPanel.GetDock(this.sidePanel));
            DockPanel.SetDock(this.sidePanel, tmpDock);
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
