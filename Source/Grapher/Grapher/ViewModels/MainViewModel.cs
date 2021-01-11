using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Grapher
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Public Members

        public ObservableCollection<Project> Projects { get; set; }

        public int SelectedProjectIndex { get; set; } = 0;

        public Canvas GraphCanvas { get; set; }
        public CanvasData GraphCanvasData { get; set; }

        #endregion


        #region Commands

        public ICommand NewProject { get; set; }
        public ICommand CloseProject { get; set; }
        public ICommand CloseAllProjects { get; set; }

        public ICommand NewGraph { get; set; }
        public ICommand EditGraph { get; set; }
        public ICommand DeleteGraph { get; set; }
        public ICommand MoveUpGraph { get; set; }
        public ICommand MoveDownGraph { get; set; }
        public ICommand TogleVisibilityGraph { get; set; }

        public ICommand FinishedSelection { get; set; }
        public ICommand ExportCanvas { get; set; }

        public ICommand WindowSizeChanged { get; set; }

        #endregion


        #region Constructor
        public MainViewModel()
        {
            /* Canvas Data */
            GraphCanvasData = new CanvasData();

            /* Project Commands */
            NewProject = new RelayCommand<object>(NewProjectExecute, NewProjectCanExecute);
            CloseProject = new RelayCommand<object>(CloseProjectExecute, CloseProjectCanExecute);
            CloseAllProjects = new RelayCommand<object>(CloseAllProjectsExecute, CloseAllProjectsCanExecute);

            /* Graph Commands */
            NewGraph = new RelayCommand<object>(NewGraphExecute, NewGraphCanExecute);
            EditGraph = new RelayCommand<object>(EditGraphExecute, EditGraphCanExecute);
            DeleteGraph = new RelayCommand<object>(DeleteGraphExecute, DeleteGraphCanExecute);
            MoveUpGraph = new RelayCommand<object>(MoveUpGraphExecute, MoveUpGraphCanExecute);
            MoveDownGraph = new RelayCommand<object>(MoveDownGraphExecute, MoveDownGraphCanExecute);
            TogleVisibilityGraph = new RelayCommand<object>(TogleVisibilityGraphExecute, TogleVisibilityGraphCanExecute);

            /* Canvas Commands */
            FinishedSelection = new RelayCommand<object>(FinishedSelectionExecute, FinishedSelectionCanExecute);
            ExportCanvas = new RelayCommand<object>(ExportCanvasExecute, ExportCanvasCanExecute);

            /* Window Commands */
            WindowSizeChanged = new RelayCommand<object>(WindowSizeChangedExecute, WindowSizeChangedCanExecute);

            /* Project Collection */
            Projects = new ObservableCollection<Project>();

            /* Subscribe to Events */
            Projects.CollectionChanged += Projects_CollectionChanged;

            GraphCanvasData.SelectionStart.PropertyChanged += RefreshCanvasProperty;
            GraphCanvasData.SelectionEnd.PropertyChanged += RefreshCanvasProperty;
        }

        #endregion


        #region Project Commands

        private bool NewProjectCanExecute(object obj)
        {
            return (Projects != null ? true : false);
        }

        private void NewProjectExecute(object obj)
        {
            Projects.Add(new Project { Name = "Project " + (Projects.Count+1).ToString()});
        }

        private bool CloseProjectCanExecute(object obj)
        {
            return (Projects.Count > 0 && SelectedProjectIndex != -1) ? true : false;
        }

        private void CloseProjectExecute(object obj)
        {
            Projects.RemoveAt(SelectedProjectIndex);
        }

        private bool CloseAllProjectsCanExecute(object obj)
        {
            return (Projects.Count > 0 ? true : false);
        }

        private void CloseAllProjectsExecute(object obj)
        {
            Projects.Clear();
        }

        #endregion


        #region Graph Commands

        private bool NewGraphCanExecute(object obj)
        {
            return (Projects.Count > 0 && SelectedProjectIndex != -1) ? true : false;
        }

        private void NewGraphExecute(object obj)
        {
            Projects[SelectedProjectIndex].Graphs.Add(new Graph { Name = "Graph " + (Projects[SelectedProjectIndex].Graphs.Count+1).ToString()});
        }

        private bool EditGraphCanExecute(object obj)
        {
            if (Projects.Count <= 0 || SelectedProjectIndex == -1)
                return false;

            return (Projects[SelectedProjectIndex].Graphs.Count > 0) ? true : false;
        }

        private void EditGraphExecute(object obj)
        {
            EditGraphViewModel editVM = new EditGraphViewModel((Graph)obj);
        }

        private bool DeleteGraphCanExecute(object obj)
        {
            if (Projects.Count <= 0 || SelectedProjectIndex == -1)
                return false;

            return (Projects[SelectedProjectIndex].Graphs.Count > 0) ? true : false;
        }

        private void DeleteGraphExecute(object obj)
        {
            Projects[SelectedProjectIndex].Graphs.Remove((Graph)obj);
        }

        private bool MoveUpGraphCanExecute(object obj)
        {
            if (Projects.Count <= 0 || SelectedProjectIndex == -1)
                return false;

            int graphIndex = Projects[SelectedProjectIndex].Graphs.IndexOf((Graph)obj);

            return (graphIndex > 0) ? true : false;
        }

        private void MoveUpGraphExecute(object obj)
        {
            int graphIndex = Projects[SelectedProjectIndex].Graphs.IndexOf((Graph)obj); ;
            Graph temp = Projects[SelectedProjectIndex].Graphs[graphIndex];

            Projects[SelectedProjectIndex].Graphs[graphIndex] = Projects[SelectedProjectIndex].Graphs[graphIndex - 1];
            Projects[SelectedProjectIndex].Graphs[graphIndex - 1] = temp;
        }

        private bool MoveDownGraphCanExecute(object obj)
        {
            if (Projects.Count <= 0 || SelectedProjectIndex == -1)
                return false;

            int graphIndex = Projects[SelectedProjectIndex].Graphs.IndexOf((Graph)obj);

            return (graphIndex < Projects[SelectedProjectIndex].Graphs.Count - 1) ? true : false;
        }

        private void MoveDownGraphExecute(object obj)
        {
            int graphIndex = Projects[SelectedProjectIndex].Graphs.IndexOf((Graph)obj); ;
            Graph temp = Projects[SelectedProjectIndex].Graphs[graphIndex];

            Projects[SelectedProjectIndex].Graphs[graphIndex] = Projects[SelectedProjectIndex].Graphs[graphIndex + 1];
            Projects[SelectedProjectIndex].Graphs[graphIndex + 1] = temp;
        }

        private bool TogleVisibilityGraphCanExecute(object obj)
        {
            if (Projects.Count <= 0 || SelectedProjectIndex == -1)
                return false;

            return (Projects[SelectedProjectIndex].Graphs.Count > 0) ? true : false;
        }

        private void TogleVisibilityGraphExecute(object obj)
        {
            if (((Graph)obj).IsVisible)
                ((Graph)obj).IsVisible = false;
            else
                ((Graph)obj).IsVisible = true;
        }

        #endregion


        #region Canvas Commands
        private bool FinishedSelectionCanExecute(object obj)
        {
            return (Projects.Count > 0 && SelectedProjectIndex != -1) ? true : false;
        }

        private void FinishedSelectionExecute(object obj)
        {
            TrimCanvas();
        }

        private bool ExportCanvasCanExecute(object obj)
        {
            return (Projects.Count > 0 && SelectedProjectIndex != -1) ? true : false;
        }

        private void ExportCanvasExecute(object obj)
        {
            /* File destination folder + name */
            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveDialog.Filter = "PNG File (*.png)|*.png|JPEG File (*.jpeg)|*.jpeg";
            

            if (saveDialog.ShowDialog() == true)
            {
                /* Canvas image render */
                RenderTargetBitmap render = new RenderTargetBitmap((int)GraphCanvas.RenderSize.Width, (int)GraphCanvas.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                render.Render(GraphCanvas);

                switch (System.IO.Path.GetExtension(saveDialog.FileName).ToLower())
                {
                    case ".png":
                        BitmapEncoder pngEncoder = new PngBitmapEncoder();
                        pngEncoder.Frames.Add(BitmapFrame.Create(render));

                        using (FileStream fs = File.OpenWrite(saveDialog.FileName))
                        {
                            pngEncoder.Save(fs);
                        }

                        break;

                    case ".jpeg":
                        BitmapEncoder jpegEncoder = new JpegBitmapEncoder();
                        jpegEncoder.Frames.Add(BitmapFrame.Create(render));

                        using (FileStream fs = System.IO.File.OpenWrite(saveDialog.FileName))
                        {
                            jpegEncoder.Save(fs);
                        }

                        break;
                    default:
                        break;
                }
            } 
        }

        #endregion


        #region Window Commands

        private bool WindowSizeChangedCanExecute(object obj)
        {
            return (Projects.Count > 0 && SelectedProjectIndex != -1) ? true : false;
        }

        private void WindowSizeChangedExecute(object obj)
        {
            PolyCalc();
            RefreshCanvas();
        }

        #endregion


        #region Events

        private void Projects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (Project project in e.NewItems)
                        project.Graphs.CollectionChanged += Graphs_CollectionChanged;
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (Project project in e.OldItems)
                        project.Graphs.CollectionChanged -= Graphs_CollectionChanged;
                    break;
                default:
                    break;
            }

        }

        private void Graphs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (Graph graph in e.NewItems)
                    {
                        graph.Points.CollectionChanged += Points_CollectionChanged;
                        graph.PropertyChanged += RefreshCanvasProperty;

                        graph.PolyExp.Monomials.CollectionChanged += Values_CollectionChanged;
                        graph.PolyExp.PropertyChanged += PolyCalcProperty;

                        RefreshCanvas();
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (Graph graph in e.OldItems)
                    {
                        graph.Points.CollectionChanged -= Points_CollectionChanged;
                        graph.PropertyChanged -= RefreshCanvasProperty;

                        graph.PolyExp.Monomials.CollectionChanged -= Values_CollectionChanged;
                        graph.PolyExp.PropertyChanged -= PolyCalcProperty;

                        RefreshCanvas();
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    RefreshCanvas();
                    break;
                default:
                    break;
            }
        }

        private void Points_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (Point2D point in e.NewItems)
                    {
                        point.PropertyChanged += RefreshCanvasProperty;

                        RefreshCanvas();
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (Point2D point in e.OldItems)
                    {
                        point.PropertyChanged -= RefreshCanvasProperty;

                        RefreshCanvas();
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    RefreshCanvas();
                    break;
                default:
                    break;
            }
        }

        private void Values_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (MonomialMember mon in e.NewItems)
                    {
                        mon.PropertyChanged += PolyCalcProperty;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (MonomialMember mon in e.OldItems)
                    {
                        mon.PropertyChanged -= PolyCalcProperty;
                    }
                    break;
                default:
                    break;
            }
        }

        private void RefreshCanvasProperty(object sender, PropertyChangedEventArgs e)
        {
            RefreshCanvas();
        }

        private void PolyCalcProperty(object sender, PropertyChangedEventArgs e)
        {
            PolyCalc();
        }

        #endregion


        #region Canvas

        public void TrimCanvas()
        {
            /* Return if no project is active */
            if (Projects.Count <= 0 || SelectedProjectIndex < 0)
                return;

            /* Select current project */
            Project currentProject;

            try
            {
                currentProject = Projects[SelectedProjectIndex];
            }
            catch (Exception)
            {
                return;
            }

            /* Local points. Avoiding negative Widths and Heights */
            Point2D localStart = GraphCanvasData.SelectionStart;
            Point2D localEnd = GraphCanvasData.SelectionEnd;
            double temp;

            /* Check which one is greater than */
            if (localStart.XValue > localEnd.XValue)
            {
                temp = localStart.XValue;
                localStart.XValue = localEnd.XValue;
                localEnd.XValue = temp;
            }

            if (localStart.YValue > localEnd.YValue)
            {
                temp = localStart.YValue;
                localStart.YValue = localEnd.YValue;
                localEnd.YValue = temp;
            }


            foreach (Graph graph in currentProject.Graphs)
            {
                for (int i = 0; i < graph.Points.Count-1; i++)
                {
                    Point2D point = graph.Points[i];

                    bool LowerX = (point.XValue < CanvasTranslator.XScreenToXReal(localStart.XValue, GraphCanvasData.xMin, GraphCanvasData.xMax, GraphCanvas.ActualWidth)) ? true : false;
                    bool HigherX = (point.XValue > CanvasTranslator.XScreenToXReal(localEnd.XValue, GraphCanvasData.xMin, GraphCanvasData.xMax, GraphCanvas.ActualWidth)) ? true : false;
                    bool LowerY = (point.YValue < CanvasTranslator.YScreenToYReal(localStart.YValue, GraphCanvasData.yMin, GraphCanvasData.yMax, GraphCanvas.ActualHeight)) ? true : false;
                    bool HigherY = (point.YValue > CanvasTranslator.YScreenToYReal(localEnd.YValue, GraphCanvasData.yMin, GraphCanvasData.yMax, GraphCanvas.ActualHeight)) ? true : false;

                    if (LowerX || HigherX || LowerY || HigherY)
                        graph.Points.Remove(point);
                }
            }

            GraphCanvasData.SelectionStart.XValue = double.NaN;
            GraphCanvasData.SelectionStart.YValue = double.NaN;
            GraphCanvasData.SelectionEnd.XValue = double.NaN;
            GraphCanvasData.SelectionEnd.YValue = double.NaN;

        }

        public void PolyCalc()
        {
            /* Return if no project is active */
            if (Projects.Count <= 0 || SelectedProjectIndex < 0)
                return;

            /* Select current project */
            Project currentProject;

            try
            {
                currentProject = Projects[SelectedProjectIndex];
            }
            catch (Exception)
            {
                return;
            }

            /* Calculate points for polynomial expression graphs */
            foreach (Graph graph in currentProject.Graphs)
            {
                if (!graph.IsAutoGenerated)
                    continue;

                if (graph.PolyExp.Monomials.Count <= 0)
                    continue;

                MathCalc.PolynomialCalculator(graph, GraphCanvas.ActualWidth);
            }
        }

        public void RefreshCanvas()
        {
            /* Clear Canvas */
            GraphCanvas.Children.Clear();

            /* Return if no project is active */
            if (Projects.Count <= 0 || SelectedProjectIndex < 0)
                return;

            /* Select current project */
            Project currentProject;

            try
            {
                currentProject = Projects[SelectedProjectIndex];
            }
            catch (Exception)
            {
                return;
            }

            /* Return if no graphs */
            if (currentProject.Graphs.Count <= 0)
                return;

            /* Local points. Avoiding negative Widths and Heights */
            Point2D localStart = GraphCanvasData.SelectionStart;
            Point2D localEnd = GraphCanvasData.SelectionEnd;

            /* Draw Selection Rectangle if needed */
            if (!double.IsNaN(localStart.XValue) && !double.IsNaN(localStart.YValue) && !double.IsNaN(localEnd.XValue) && !double.IsNaN(localEnd.YValue))
            {
                double temp;

                if ((localEnd.XValue - localStart.XValue - 1) >= 0 && (localEnd.YValue - localStart.YValue - 1) >= 0)
                {
                    /* Check which one is greater than */
                    if (localStart.XValue > localEnd.XValue)
                    {
                        temp = localStart.XValue;
                        localStart.XValue = localEnd.XValue;
                        localEnd.XValue = temp;
                    }

                    if (localStart.YValue > localEnd.YValue)
                    {
                        temp = localStart.YValue;
                        localStart.YValue = localEnd.YValue;
                        localEnd.YValue = temp;
                    }

                    Rectangle selectionRect = new Rectangle();

                    selectionRect.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#40303030"));
                    selectionRect.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#40323232"));
                    selectionRect.StrokeThickness = 1;

                    Canvas.SetTop(selectionRect, localStart.YValue);
                    Canvas.SetLeft(selectionRect, localStart.XValue);
                    selectionRect.Width = localEnd.XValue - localStart.XValue - 1;
                    selectionRect.Height = localEnd.YValue - localStart.YValue - 1;

                    GraphCanvas.Children.Add(selectionRect);
                }
            }


            /* Serach for points as reference */
            foreach (Graph graph in currentProject.Graphs)
            {
                if (!graph.IsVisible)
                    continue;

                if (graph.Points.Count > 0)
                {
                    GraphCanvasData.xMin = graph.Points[0].XValue;
                    GraphCanvasData.xMax = graph.Points[0].XValue;
                    GraphCanvasData.yMin = graph.Points[0].YValue;
                    GraphCanvasData.yMax = graph.Points[0].YValue;

                    break;
                }
            }

            /* Return if no points */
            if (double.IsNaN(GraphCanvasData.xMax) || double.IsNaN(GraphCanvasData.xMin) || double.IsNaN(GraphCanvasData.yMax) || double.IsNaN(GraphCanvasData.yMin))
                return;

            /* Search Max and Min values */
            foreach (Graph graph in currentProject.Graphs)
            {
                if (!graph.IsVisible)
                    continue;

                foreach (Point2D point in graph.Points)
                {
                    GraphCanvasData.xMin = (GraphCanvasData.xMin > point.XValue) ? point.XValue : GraphCanvasData.xMin;
                    GraphCanvasData.xMax = (GraphCanvasData.xMax < point.XValue) ? point.XValue : GraphCanvasData.xMax;
                    GraphCanvasData.yMin = (GraphCanvasData.yMin > point.YValue) ? point.YValue : GraphCanvasData.yMin;
                    GraphCanvasData.yMax = (GraphCanvasData.yMax < point.YValue) ? point.YValue : GraphCanvasData.yMax;
                }
            }

            /* Draw Axis */
            try
            {
                Line XAxis = new Line()
                {
                    X1 = 0,
                    Y1 = CanvasTranslator.YRealToYScreen(0, GraphCanvasData.yMin, GraphCanvasData.yMax, GraphCanvas.ActualHeight),
                    X2 = GraphCanvas.ActualWidth,
                    Y2 = CanvasTranslator.YRealToYScreen(0, GraphCanvasData.yMin, GraphCanvasData.yMax, GraphCanvas.ActualHeight),
                    Stroke = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("#272727") },
                    StrokeThickness = 1
                };
                GraphCanvas.Children.Add(XAxis);

                Line YAxis = new Line()
                {
                    X1 = CanvasTranslator.XRealToXScreen(0, GraphCanvasData.xMin, GraphCanvasData.xMax, GraphCanvas.ActualWidth),
                    Y1 = 0,
                    X2 = CanvasTranslator.XRealToXScreen(0, GraphCanvasData.xMin, GraphCanvasData.xMax, GraphCanvas.ActualWidth),
                    Y2 = GraphCanvas.ActualHeight,
                    Stroke = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("#272727") },
                    StrokeThickness = 1
                };
                GraphCanvas.Children.Add(YAxis);
            }
            catch (Exception)
            {

            }

            /* Draw shapes depending of graph type */
            for (int i = currentProject.Graphs.Count - 1; i >= 0; i--)
            {
                Graph graph = currentProject.Graphs[i];

                switch (graph.Type)
                {
                    #region LineGraph
                    case GraphType.LineGraph:

                        Polyline tmpPoly = new Polyline();

                        foreach (Point2D point in graph.Points)
                            tmpPoly.Points.Add(new Point
                            {
                                X = CanvasTranslator.XRealToXScreen(point.XValue, GraphCanvasData.xMin, GraphCanvasData.xMax, GraphCanvas.ActualWidth),
                                Y = CanvasTranslator.YRealToYScreen(point.YValue, GraphCanvasData.yMin, GraphCanvasData.yMax, GraphCanvas.ActualHeight)
                            });

                        switch (graph.Dash)
                        {
                            case DashType.Dot:
                                tmpPoly.StrokeDashArray = new DoubleCollection() { 1, 2 };
                                break;
                            case DashType.DottedPlus:
                                tmpPoly.StrokeDashArray = new DoubleCollection() { 1, 6 };
                                break;
                            case DashType.Dashed:
                                tmpPoly.StrokeDashArray = new DoubleCollection() { 6, 1 };
                                break;
                            case DashType.DotDash:
                                tmpPoly.StrokeDashArray = new DoubleCollection() { 4, 1, 1, 1 };
                                break;
                            case DashType.DotDotDash:
                                tmpPoly.StrokeDashArray = new DoubleCollection() { 4, 1, 1, 1, 1, 1 };
                                break;
                            default:
                                break;
                        }

                        tmpPoly.Opacity = graph.Opacity;
                        tmpPoly.StrokeDashCap = graph.Cap;
                        tmpPoly.StrokeStartLineCap = tmpPoly.StrokeEndLineCap = graph.Cap;
                        tmpPoly.StrokeThickness = graph.Thickness;
                        tmpPoly.Stroke = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(graph.Color) };
                        tmpPoly.Visibility = graph.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;

                        GraphCanvas.Children.Add(tmpPoly);

                        break;
                    #endregion

                    #region BarGraph
                    case GraphType.BarGraph:

                        foreach (Point2D point in graph.Points)
                        {
                            Line tmpLine = new Line();

                            try
                            {
                                if (double.IsNaN(CanvasTranslator.XRealToXScreen(point.XValue, GraphCanvasData.xMin, GraphCanvasData.xMax, GraphCanvas.ActualWidth)))
                                    tmpLine.X1 = GraphCanvas.ActualWidth / 2;
                                else
                                    tmpLine.X1 = CanvasTranslator.XRealToXScreen(point.XValue, GraphCanvasData.xMin, GraphCanvasData.xMax, GraphCanvas.ActualWidth);

                                if (double.IsNaN(CanvasTranslator.YRealToYScreen(0, GraphCanvasData.yMin, GraphCanvasData.yMax, GraphCanvas.ActualHeight)))
                                    tmpLine.Y1 = GraphCanvas.ActualHeight;
                                else
                                    tmpLine.Y1 = CanvasTranslator.YRealToYScreen(0, GraphCanvasData.yMin, GraphCanvasData.yMax, GraphCanvas.ActualHeight);

                                if (double.IsNaN(CanvasTranslator.XRealToXScreen(point.XValue, GraphCanvasData.xMin, GraphCanvasData.xMax, GraphCanvas.ActualWidth)))
                                    tmpLine.X2 = GraphCanvas.ActualWidth / 2;
                                else
                                    tmpLine.X2 = CanvasTranslator.XRealToXScreen(point.XValue, GraphCanvasData.xMin, GraphCanvasData.xMax, GraphCanvas.ActualWidth);

                                if (double.IsNaN(CanvasTranslator.YRealToYScreen(point.YValue, GraphCanvasData.yMin, GraphCanvasData.yMax, GraphCanvas.ActualHeight)))
                                    tmpLine.Y2 = 0;
                                else
                                    tmpLine.Y2 = CanvasTranslator.YRealToYScreen(point.YValue, GraphCanvasData.yMin, GraphCanvasData.yMax, GraphCanvas.ActualHeight);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            
                            switch (graph.Dash)
                            {
                                case DashType.Dot:
                                    tmpLine.StrokeDashArray = new DoubleCollection() { 1 };
                                    break;
                                case DashType.DottedPlus:
                                    tmpLine.StrokeDashArray = new DoubleCollection() { 1, 6 };
                                    break;
                                case DashType.Dashed:
                                    tmpLine.StrokeDashArray = new DoubleCollection() { 6, 1 };
                                    break;
                                case DashType.DotDash:
                                    tmpLine.StrokeDashArray = new DoubleCollection() { 4, 1, 1, 1 };
                                    break;
                                case DashType.DotDotDash:
                                    tmpLine.StrokeDashArray = new DoubleCollection() { 4, 1, 1, 1, 1, 1 };
                                    break;
                                default:
                                    break;
                            }

                            tmpLine.Opacity = graph.Opacity;
                            tmpLine.StrokeDashCap = graph.Cap;
                            tmpLine.StrokeStartLineCap = tmpLine.StrokeEndLineCap = graph.Cap;
                            tmpLine.StrokeThickness = graph.Thickness;
                            tmpLine.Stroke = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(graph.Color) };
                            tmpLine.Visibility = graph.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;

                            GraphCanvas.Children.Add(tmpLine);
                        }
                        break;
                    #endregion

                    default:
                        break;
                }
            }

        }

        #endregion
    }
}
