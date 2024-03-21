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
        private bool            readOnly;
        private bool            hidden;
        private DateTime        creation;
        private DateTime        modfied;
        private long            size;
        private FolderItemType  type;

        public ImageSource Icon { get; private set; }
        public FolderItemType Type { get => type; set { type = value; OnPropertyChanged(nameof(Type));  } }
        public long Size { get => size; set { size = value; OnPropertyChanged(nameof(Size)); } }
        public DateTime Creation { get => creation; set { creation = value; OnPropertyChanged(nameof(Creation)); } }
        public DateTime LastModified { get => modfied; set { modfied = value; OnPropertyChanged(nameof(LastModified)); } }
        public string Path { get => path; set { path = value; OnPropertyChanged(nameof(Path)); } }
        public bool ReadOnly { get => readOnly; set { readOnly = value; OnPropertyChanged(nameof(ReadOnly)); } }
        public bool Hidden { get => hidden; set { hidden = value; OnPropertyChanged(nameof(Hidden)); } }
        public string Name => System.IO.Path.GetFileName(path);

        public FolderItem(FolderItemType type, long size, DateTime creation, DateTime modified, bool readOnly, bool hidden, string path)
        {
            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(Path))
                    Icon = Util.IconHandler.GetIcon(path, true);
            };
            Type = type;
            Size = size;
            Creation = creation;
            ReadOnly = readOnly;
            LastModified = modified;
            Path = path;
            Hidden = hidden;
        }
    }
}
