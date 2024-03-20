using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Data;
using Rover.Models;

namespace Rover.Composition
{
    public class ObservableFolderContent
    {
        public ObservableCollection<FolderItem> Items { get; private set; } = new ObservableCollection<FolderItem>();

        private string path;
        public string Path
        {
            get => path;
            set
            {
                path = value;
                Reload(path);
            }
        }

        private FileSystemWatcher watcher;

        public ObservableFolderContent()
        {
            Path = Environment.CurrentDirectory;
        }

        ~ObservableFolderContent()
        {
            if (watcher != null)
                watcher.Dispose();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
                return;
            Console.WriteLine($"Changed: {e.FullPath}");
            Reload(e.FullPath);
        }

        public bool Reload(string path)
        {
            if (!Directory.Exists(path))
                return false;
            this.path = path;
            if (watcher != null)
                watcher.Dispose();
            watcher = new FileSystemWatcher(Path);
            watcher.NotifyFilter = NotifyFilters.Attributes
                              | NotifyFilters.CreationTime
                              | NotifyFilters.DirectoryName
                              | NotifyFilters.FileName
                              | NotifyFilters.LastAccess
                              | NotifyFilters.LastWrite
                              | NotifyFilters.Security
                              | NotifyFilters.Size;
            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;

            Items.Clear();
            string[] subdirectories = System.IO.Directory.GetDirectories(Path);
            foreach (var item in subdirectories)
            {
                var info = new DirectoryInfo(item);
                Items.Add(new FolderItem(FolderItem.FolderItemType.Folder, 0, info.CreationTime, item));
            }

            string[] files = System.IO.Directory.GetFiles(Path);
            foreach (var item in files)
            {
                var info = new FileInfo(item);
                Items.Add(new FolderItem(FolderItem.FolderItemType.File, info.Length, info.CreationTime, item));
            }
            ICollectionView view = CollectionViewSource.GetDefaultView(Items);
            view.Refresh();
            return true;
        }
    }
}