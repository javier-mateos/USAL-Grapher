using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Grapher
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Random rnd;

        #region Commands

        public ICommand NewProject { get; set; }
        public ICommand CloseProject { get; set; }
        public ICommand CloseAllProjects { get; set; }

        public ICommand NewGraph { get; set; }
        public ICommand MoveUpGraph { get; set; }
        public ICommand MoveDownGraph { get; set; }
        public ICommand DeleteGraph { get; set; }

        #endregion


        #region Private Members

        #endregion


        #region Public Members

        public int SelectedProjectIndex { get; set; } = 0;
        public int SelectedGraphIndex { get; set; } = 0;


        public ObservableCollection<Project> Projects { get; set; }

        #endregion

        public MainWindowViewModel()
        {
            NewProject = new RelayCommand<object>(NewProjectExecute, NewProjectCanExecute);
            CloseProject = new RelayCommand<object>(CloseProjectExecute, CloseProjectCanExecute);
            CloseAllProjects = new RelayCommand<object>(CloseAllProjectsExecute, CloseAllProjectsCanExecute);

            NewGraph = new RelayCommand<object>(NewGraphExecute, NewGraphCanExecute);
            MoveUpGraph = new RelayCommand<object>(MoveUpGraphExecute, MoveUpGraphCanExecute);
            MoveDownGraph = new RelayCommand<object>(MoveDownGraphExecute, MoveDownGraphCanExecute);
            DeleteGraph = new RelayCommand<object>(DeleteGraphExecute, DeleteGraphCanExecute);

            Projects = new ObservableCollection<Project>();
            rnd = new Random();
        }

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


        private bool NewGraphCanExecute(object obj)
        {
            return (Projects.Count > 0 ? true : false);
        }

        private void NewGraphExecute(object obj)
        {
            Projects[SelectedProjectIndex].Graphs.Add(new Graph { Name = "Test Graph " + rnd.Next(10) });
        }

        private bool MoveUpGraphCanExecute(object obj)
        {
            if (Projects.Count <= 0)
                return false;

            int graphIndex = Projects[SelectedProjectIndex].Graphs.IndexOf((Graph)obj);

            return graphIndex > 0 ? true : false;
        }

        private void MoveUpGraphExecute(object obj)
        {
            if (Projects.Count <= 0)
                return;

            int graphIndex = Projects[SelectedProjectIndex].Graphs.IndexOf((Graph)obj); ;
            Graph temp = Projects[SelectedProjectIndex].Graphs[graphIndex];
            
            Projects[SelectedProjectIndex].Graphs[graphIndex] = Projects[SelectedProjectIndex].Graphs[graphIndex - 1];
            Projects[SelectedProjectIndex].Graphs[graphIndex - 1] = temp;
        }

        private bool MoveDownGraphCanExecute(object obj)
        {
            if (Projects.Count <= 0)
                return false;

            int graphIndex = Projects[SelectedProjectIndex].Graphs.IndexOf((Graph)obj);

            return graphIndex < Projects[SelectedProjectIndex].Graphs.Count - 1 ? true : false;
        }

        private void MoveDownGraphExecute(object obj)
        {
            if (Projects.Count <= 0)
                return;

            int graphIndex = Projects[SelectedProjectIndex].Graphs.IndexOf((Graph)obj); ;
            Graph temp = Projects[SelectedProjectIndex].Graphs[graphIndex];

            Projects[SelectedProjectIndex].Graphs[graphIndex] = Projects[SelectedProjectIndex].Graphs[graphIndex + 1];
            Projects[SelectedProjectIndex].Graphs[graphIndex + 1] = temp;
        }

        private bool DeleteGraphCanExecute(object obj)
        {
            return (Projects.Count > 0 && Projects[SelectedProjectIndex].Graphs.Count > 0 ? true : false);
        }

        private void DeleteGraphExecute(object obj)
        {
            Projects[SelectedProjectIndex].Graphs.Remove((Graph)obj);
        }
    }
}
