using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Grapher
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Random rnd;

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

        public ICommand WindowResized { get; set; }
        

        #endregion


        #region Public Members

        public int SelectedProjectIndex { get; set; } = 0;

        public int CanvasHeight { get; set; }
        public int CanvasWidth { get; set; }

        public int[] CanvasSize 
        {
            get
            {
                CanvasSize[0] = CanvasHeight;
                CanvasSize[1] = CanvasWidth;

                return CanvasSize;
            }
        }

        public ObservableCollection<Project> Projects { get; set; }

        #endregion


        #region Constructor
        public MainViewModel()
        {
            /* Temp */
            rnd = new Random();

            NewProject = new RelayCommand<object>(NewProjectExecute, NewProjectCanExecute);
            CloseProject = new RelayCommand<object>(CloseProjectExecute, CloseProjectCanExecute);
            CloseAllProjects = new RelayCommand<object>(CloseAllProjectsExecute, CloseAllProjectsCanExecute);

            NewGraph = new RelayCommand<object>(NewGraphExecute, NewGraphCanExecute);
            EditGraph = new RelayCommand<object>(EditGraphExecute, EditGraphCanExecute);
            DeleteGraph = new RelayCommand<object>(DeleteGraphExecute, DeleteGraphCanExecute);
            MoveUpGraph = new RelayCommand<object>(MoveUpGraphExecute, MoveUpGraphCanExecute);
            MoveDownGraph = new RelayCommand<object>(MoveDownGraphExecute, MoveDownGraphCanExecute);
            TogleVisibilityGraph = new RelayCommand<object>(TogleVisibilityGraphExecute, TogleVisibilityGraphCanExecute);

            WindowResized = new RelayCommand<object>(WindowResizedExecute, WindowResizedCanExecute);
            
            Projects = new ObservableCollection<Project>();
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
            return (Projects.Count > 0 ? true : false);
        }

        private void CloseProjectExecute(object obj)
        {
            Projects.Remove((Project)obj);
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

        private bool WindowResizedCanExecute(object obj)
        {
            return true;
        }

        private void WindowResizedExecute(object obj)
        {
            
        }

        #endregion

        #region Canvas Functions

        /*void canvasTranslation()
        {
            if (SelectedProjectIndex == -1)
                return;

            CanvasDataTranslation.Clear();

            foreach (Graph graph in Projects[SelectedProjectIndex].Graphs)
            {
                switch (graph.Type)
                {
                    case GraphType.LineGraph:

                        Polyline newLine = new Polyline();

                        newLine.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(graph.Color));
                        newLine.Opacity = graph.Opacity;
                        newLine.StrokeThickness = graph.Thickness;

                        foreach (Point2D point in graph.Points)
                        {
                            newLine.Points.Add(new System.Windows.Point { X = point.XValue, Y = point.YValue });
                        }

                        CanvasDataTranslation.Add(newLine);

                        break;

                    case GraphType.BarGraph:



                        break;

                    default:
                        return;
                }
            }
            
        }*/

        #endregion
    }
}
