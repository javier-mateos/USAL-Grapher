using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Grapher
{
    public partial class MainWindow : Window
    {
        MainViewModel MainVm { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MainVm = new MainViewModel();

            this.DataContext = MainVm;
            MainVm.GraphCanvas = graphCanvas;

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

        private void projectsTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;

            MainVm.RefreshCanvas();
        }
    }
}
