using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Interop;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Rover.Models
{
    public class FolderItem : ObservableObject
    {
        public enum FolderItemType
        { 
            Folder,
            File
        };

        private string          path;
        private DateTime        creation;
        private long            size;
        private FolderItemType  type;

        public ImageSource Icon { get; private set; }
        public FolderItemType Type { get => type; set { type = value; OnPropertyChanged(nameof(Type));  } }
        public long Size { get => size; set { size = value; OnPropertyChanged(nameof(Size)); } }
        public DateTime Creation { get => creation; set { creation = value; OnPropertyChanged(nameof(Creation)); } }
        public string Path { get => path; set { path = value; OnPropertyChanged(nameof(Path)); } }
        public string Name => System.IO.Path.GetFileName(path);

        public FolderItem(FolderItemType type, long size, DateTime creation, string path)
        {
            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(Path))
                    Icon = Util.IconHandler.GetIcon(path, true);
            };
            Type = type;
            Size = size;
            Creation = creation;
            Path = path;
        }
    }
}
