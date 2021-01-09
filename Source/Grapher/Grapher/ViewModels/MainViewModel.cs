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

        Random rnd;


        #region Public Members

        public ObservableCollection<Project> Projects { get; set; }

        public int SelectedProjectIndex { get; set; } = 0;
        
        public Canvas GraphCanvas { get; set; }

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
            /* Temp */
            rnd = new Random();

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

            /* Subscribe to Project CollectionChanged Event */
            Projects.CollectionChanged += Projects_CollectionChanged;

        }

        #endregion


        #region Project Commands

        private bool NewProjectCanExecute(object obj)
        {
            return (Projects != null ? true : false);
        }

        private void NewProjectExecute(object obj)
        {
            Projects.Add(new Project { Name = "Test Project " + rnd.Next(10) });
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
            Projects[SelectedProjectIndex].Graphs.Add(new Graph { Name = "Test Graph " + rnd.Next(10) });
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
                    {
                        project.Graphs.CollectionChanged += Graphs_CollectionChanged;

                        RefreshCanvas();
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (Project project in e.OldItems)
                    {
                        project.Graphs.CollectionChanged -= Graphs_CollectionChanged;

                        RefreshCanvas();
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

        private void Graphs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (Graph graph in e.NewItems)
                    {
                        graph.Points.CollectionChanged += RefreshCanvasCollection;
                        graph.PropertyChanged += RefreshCanvasProperty;

                        RefreshCanvas();
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (Graph graph in e.OldItems)
                    {
                        graph.Points.CollectionChanged -= RefreshCanvasCollection;
                        graph.PropertyChanged -= RefreshCanvasProperty;

                        RefreshCanvas();
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    RefreshCanvas();
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    RefreshCanvas();

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
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
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        private void RefreshCanvasCollection(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RefreshCanvas();
        }

        private void RefreshCanvasProperty(object sender, PropertyChangedEventArgs e)
        {
            RefreshCanvas();
        }

        #endregion


        #region Canvas

        public void RefreshCanvas()
        {
            GraphCanvas.Children.Clear();

            if (Projects.Count <= 0 || SelectedProjectIndex < 0)
                return;

            Project currentProject;

            try
            {
                currentProject = Projects[SelectedProjectIndex];
            }
                
            catch (Exception)
            {
                return;
            }


            if (currentProject.Graphs.Count <= 0)
                return;

            if (currentProject.Graphs[0].Points.Count <= 0)
                return;


            double xMin = currentProject.Graphs[0].Points[0].XValue;
            double xMax = currentProject.Graphs[0].Points[0].XValue;
            double yMin = currentProject.Graphs[0].Points[0].YValue;
            double yMax = currentProject.Graphs[0].Points[0].YValue;

            foreach (Graph tmpGraph in Projects[SelectedProjectIndex].Graphs)
            {
                if (!tmpGraph.IsVisible)
                    continue;

                foreach (Point2D point in tmpGraph.Points)
                {
                    xMin = (xMin > point.XValue) ? point.XValue : xMin;
                    xMax = (xMax < point.XValue) ? point.XValue : xMax;
                    yMin = (yMin > point.YValue) ? point.YValue : yMin;
                    yMax = (yMax < point.YValue) ? point.YValue : yMax;
                }
            }

            for (int i = Projects[SelectedProjectIndex].Graphs.Count-1; i >= 0; i--)
            {
                Graph graph = Projects[SelectedProjectIndex].Graphs[i];

                switch (graph.Type)
                {
                    case GraphType.LineGraph:
                        
                        Polyline tmpPoly = new Polyline();

                        foreach (Point2D point in graph.Points)
                            tmpPoly.Points.Add(new Point {
                                X = CanvasTranslator.XRealToXScreen(point.XValue, xMin, xMax, GraphCanvas.ActualWidth), 
                                Y = CanvasTranslator.YRealToYScreen(point.YValue, yMin, yMax, GraphCanvas.ActualHeight)
                            });

                        tmpPoly.Opacity = graph.Opacity;
                        tmpPoly.StrokeThickness = graph.Thickness;
                        tmpPoly.Stroke = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(graph.Color) };
                        tmpPoly.Visibility = graph.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;

                        GraphCanvas.Children.Add(tmpPoly);

                        break;
                    case GraphType.BarGraph:
                        foreach (Point2D point in graph.Points)
                        {
                            Line tmpLine = new Line();

                            tmpLine.X1 = CanvasTranslator.XRealToXScreen(point.XValue, xMin, xMax, GraphCanvas.ActualWidth);
                            tmpLine.Y1 = GraphCanvas.ActualHeight;
                            tmpLine.X2 = CanvasTranslator.XRealToXScreen(point.XValue, xMin, xMax, GraphCanvas.ActualWidth);
                            tmpLine.Y2 = CanvasTranslator.YRealToYScreen(point.YValue, yMin, yMax, GraphCanvas.ActualHeight);

                            tmpLine.Opacity = graph.Opacity;
                            tmpLine.StrokeThickness = graph.Thickness;
                            tmpLine.Stroke = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(graph.Color) };
                            tmpLine.Visibility = graph.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;

                            GraphCanvas.Children.Add(tmpLine);
                        }
                        break;
                    default:
                        break;
                }
            }

            
        }

        #endregion
    }
}
