using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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


        public Point2D SelectionStart { get; set; }
        public Point2D SelectionEnd { get; set; }

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

        public ICommand WindowSizeChanged { get; set; }

        #endregion


        #region Constructor
        public MainViewModel()
        {
            /* Selection Points */
            SelectionStart = new Point2D { XValue = double.NaN, YValue = double.NaN };
            SelectionEnd = new Point2D { XValue = double.NaN, YValue = double.NaN };

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

            /* Window Commands */
            WindowSizeChanged = new RelayCommand<object>(WindowSizeChangedExecute, WindowSizeChangedCanExecute);

            /* Project Collection */
            Projects = new ObservableCollection<Project>();

            /* Subscribe to Events */
            Projects.CollectionChanged += Projects_CollectionChanged;

            SelectionStart.PropertyChanged += RefreshCanvasProperty;
            SelectionEnd.PropertyChanged += RefreshCanvasProperty;
        }

        #endregion


        #region Project Commands

        private bool NewProjectCanExecute(object obj)
        {
            return (Projects != null ? true : false);
        }

        private void NewProjectExecute(object obj)
        {
            Projects.Add(new Project { Name = "New Project" });
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
            Projects[SelectedProjectIndex].Graphs.Add(new Graph { Name = "New Graph"});
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


        #region Window Commands

        private bool WindowSizeChangedCanExecute(object obj)
        {
            return (Projects.Count > 0 && SelectedProjectIndex != -1) ? true : false;
        }

        private void WindowSizeChangedExecute(object obj)
        {
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
                        graph.PolyExp.PropertyChanged += RefreshCanvasProperty;

                        RefreshCanvas();
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (Graph graph in e.OldItems)
                    {
                        graph.Points.CollectionChanged -= Points_CollectionChanged;
                        graph.PropertyChanged -= RefreshCanvasProperty;

                        graph.PolyExp.Monomials.CollectionChanged -= Values_CollectionChanged;
                        graph.PolyExp.PropertyChanged -= RefreshCanvasProperty;

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
                        mon.PropertyChanged += RefreshCanvasProperty;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (MonomialMember mon in e.OldItems)
                    {
                       mon.PropertyChanged -= RefreshCanvasProperty;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        private void RefreshCanvasProperty(object sender, PropertyChangedEventArgs e)
        {
            RefreshCanvas();
        }

        #endregion


        #region Canvas

        public void RefreshCanvas()
        {
            /* Canvas Center Point */
            double XCenter = GraphCanvas.ActualWidth / 2;
            double YCenter = GraphCanvas.ActualHeight / 2;

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

            /* Draw Selection Rectangle if needed */
            if(!double.IsNaN(SelectionStart.XValue) && !double.IsNaN(SelectionStart.YValue) && !double.IsNaN(SelectionEnd.XValue) && !double.IsNaN(SelectionEnd.YValue))
            {
                /* Local points. Avoiding negative Widths and Heights */
                Point2D localStart = SelectionStart;
                Point2D localEnd = SelectionEnd;
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

            /* Calculate points for polynomial expression graphs */
            foreach (Graph graph in currentProject.Graphs)
            {
                if (!graph.IsAutoGenerated)
                    continue;

                if (graph.PolyExp.Monomials.Count <= 0)
                    continue;

                graph.Points.Clear();

                MathCalc.PolynomialCalculator(graph.PolyExp, GraphCanvas.ActualWidth);
            }

            /* Current Graph Point List (To read both normal and auto-generated graphs). ObservableCollection for avoiding castings */
            ObservableCollection<Point2D> pointList = new ObservableCollection<Point2D>();

            /* X and Y variables */
            double xMin, xMax, yMin, yMax;
            xMax = xMin = yMax = yMin = double.NaN;

            /* Serach for points as reference */
            foreach (Graph graph in currentProject.Graphs)
            {
                if (!graph.IsVisible)
                    continue;

                if (graph.IsAutoGenerated)
                    pointList = graph.PolyExp.CalculatedPoints;
                else
                    pointList = graph.Points;

                if (pointList.Count > 0)
                {
                    xMin = pointList[0].XValue;
                    xMax = pointList[0].XValue;
                    yMin = pointList[0].YValue;
                    yMax = pointList[0].YValue;

                    break;
                }
            }

            /* Return if no points */
            if (double.IsNaN(xMax) || double.IsNaN(xMin) || double.IsNaN(yMax) || double.IsNaN(yMin))
                return;

            /* Search Max and Min values */
            foreach (Graph graph in currentProject.Graphs)
            {
                if (!graph.IsVisible)
                    continue;

                if (graph.IsAutoGenerated)
                    pointList = graph.PolyExp.CalculatedPoints;
                else
                    pointList = graph.Points;

                foreach (Point2D point in pointList)
                {
                    xMin = (xMin > point.XValue) ? point.XValue : xMin;
                    xMax = (xMax < point.XValue) ? point.XValue : xMax;
                    yMin = (yMin > point.YValue) ? point.YValue : yMin;
                    yMax = (yMax < point.YValue) ? point.YValue : yMax;
                }
            }

            try
            {
                /* Draw Axis */
                Line XAxis = new Line()
                {
                    X1 = 0,
                    Y1 = CanvasTranslator.YRealToYScreen(0, yMin, yMax, GraphCanvas.ActualHeight),
                    X2 = GraphCanvas.ActualWidth,
                    Y2 = CanvasTranslator.YRealToYScreen(0, yMin, yMax, GraphCanvas.ActualHeight),
                    Stroke = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("#272727") },
                    StrokeThickness = 1
                };
                GraphCanvas.Children.Add(XAxis);

                Line YAxis = new Line()
                {
                    X1 = CanvasTranslator.XRealToXScreen(0, xMin, xMax, GraphCanvas.ActualWidth),
                    Y1 = 0,
                    X2 = CanvasTranslator.XRealToXScreen(0, xMin, xMax, GraphCanvas.ActualWidth),
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

                        if (graph.IsAutoGenerated)
                            pointList = graph.PolyExp.CalculatedPoints;
                        else
                            pointList = graph.Points;

                        foreach (Point2D point in pointList)
                            tmpPoly.Points.Add(new Point
                            {
                                X = CanvasTranslator.XRealToXScreen(point.XValue, xMin, xMax, GraphCanvas.ActualWidth),
                                Y = CanvasTranslator.YRealToYScreen(point.YValue, yMin, yMax, GraphCanvas.ActualHeight)
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

                        if (graph.IsAutoGenerated)
                            pointList = graph.PolyExp.CalculatedPoints;
                        else
                            pointList = graph.Points;

                        foreach (Point2D point in pointList)
                        {
                            Line tmpLine = new Line();

                            tmpLine.X1 = CanvasTranslator.XRealToXScreen(point.XValue, xMin, xMax, GraphCanvas.ActualWidth);
                            tmpLine.Y1 = GraphCanvas.ActualHeight;
                            tmpLine.X2 = CanvasTranslator.XRealToXScreen(point.XValue, xMin, xMax, GraphCanvas.ActualWidth);
                            tmpLine.Y2 = CanvasTranslator.YRealToYScreen(point.YValue, yMin, yMax, GraphCanvas.ActualHeight);

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
