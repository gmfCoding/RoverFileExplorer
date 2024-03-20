﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rover.Models;
using System.Windows.Input;
using Rover.Composition;

namespace Rover.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ObservableFolderContent Folder { get; private set; } = new ObservableFolderContent();

        private const int maxHistory = 25;
        private List<string> history = new(maxHistory);
        private int historyIndex;

        private string path = "";

        /// <summary>
        /// When modified sets the new path to the new value but doesn't update the history. <br/>
        /// Reloads ObersableFolderContentFolder
        /// </summary>
        private string CurrentPath
        {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged(nameof(Path));
                OnPathSet(path, false);
            }
        }

        /// <summary>
        /// When modified sets the new path but only updates this history if it is a valid folder path. <br/>
        /// Reloads ObersableFolderContentFolder
        /// </summary>
        public string Path { get => path;
            set {
                path = value;
                OnPropertyChanged(nameof(Path));
                OnPathSet(path, System.IO.Directory.Exists(path));
            }
        }

        public RelayCommand HistoryBack { get; private set; }
        public RelayCommand HistoryForward { get; private set; }
        public RelayCommand ParentFolder { get; private set; }

        public bool CanHistoryBack => history != null && historyIndex < history.Count - 1;
        public bool CanHistoryFoward => history != null && historyIndex > 0;

        public MainViewModel()
        {
            Path = Environment.CurrentDirectory;
            SetupCommands();
        }

        private void SetupCommands()
        {
            HistoryBack = new RelayCommand(() =>
            {
                historyIndex = Math.Clamp(historyIndex + 1, 0, history.Count - 1);
                CurrentPath = history[historyIndex];
                NotifyCanExecuteChanged();
            }, () => CanHistoryBack);

            HistoryForward = new RelayCommand(() =>
            {
                historyIndex = Math.Clamp(historyIndex - 1, 0, history.Count - 1);
                CurrentPath = history[historyIndex];
                NotifyCanExecuteChanged();
            }, () => CanHistoryFoward);

            ParentFolder = new RelayCommand(() =>
            {
                Path = System.IO.Directory.GetParent(CurrentPath).FullName;
                System.Diagnostics.Trace.WriteLine(Path);
                NotifyCanExecuteChanged();
            });
        }

        private void NotifyCanExecuteChanged()
        {
            HistoryForward?.NotifyCanExecuteChanged();
            HistoryBack?.NotifyCanExecuteChanged();
        }

        public void OnPathSet(string path, bool recordHistory)
        {
            if (recordHistory)
                AddPathHistory(path);
            Folder.Reload(path);
        }

        public void AddPathHistory(string path)
        {
            if (!System.IO.Directory.Exists(path))
                return;

            if (history.Count > 0 && history[0] == path)
                return;

            history.Insert(0, path);
            if (history.Count > maxHistory)
                history.RemoveAt(history.Count - 1);
            NotifyCanExecuteChanged();
        }
    }
}
