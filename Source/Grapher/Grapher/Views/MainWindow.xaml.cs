using System.Windows;
using System.Windows.Controls;

namespace Grapher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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

        private void addGraphBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void closeProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
