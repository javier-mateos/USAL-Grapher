using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Grapher
{
    public partial class MainWindow : Window
    {
        MainViewModel MainVM { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MainVM = new MainViewModel();

            this.DataContext = MainVM;
            MainVM.GraphCanvas = graphCanvas;

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

            MainVM.RefreshCanvas();
        }

        private void graphCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainVM.SelectionStart.XValue = e.GetPosition(graphCanvas).X;
            MainVM.SelectionStart.YValue = e.GetPosition(graphCanvas).Y;
            MainVM.SelectionEnd.XValue = double.NaN;
            MainVM.SelectionEnd.YValue = double.NaN;
        }

        private void graphCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton != System.Windows.Input.MouseButtonState.Pressed)
                return;

            MainVM.SelectionEnd.XValue = e.GetPosition(graphCanvas).X;
            MainVM.SelectionEnd.YValue = e.GetPosition(graphCanvas).Y;
        }
    }
}
