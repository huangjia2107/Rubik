using System.Linq;
using System.Collections.ObjectModel;

using Rubik.Toolkit.Datas;

namespace Rubik.Demo.DragList.ViewModels
{
    public class TreeViewModel : TreeViewModelBase
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        private bool _isDragSource;
        public bool IsDragSource
        {
            get { return _isDragSource; }
            set { SetProperty(ref _isDragSource, value); }
        }

        private bool _isDragOver;
        public bool IsDragOver
        {
            get { return _isDragOver; }
            set { SetProperty(ref _isDragOver, value); }
        }

        private bool _isDragOverUp;
        public bool IsDragOverUp
        {
            get { return _isDragOverUp; }
            set { SetProperty(ref _isDragOverUp, value); }
        }

        private bool _isDragOverDown;
        public bool IsDragOverDown
        {
            get { return _isDragOverDown; }
            set { SetProperty(ref _isDragOverDown, value); }
        }

        private bool _isDragOverLeft;
        public bool IsDragOverLeft
        {
            get { return _isDragOverLeft; }
            set { SetProperty(ref _isDragOverLeft, value); }
        }

        private bool _isDragOverRight;
        public bool IsDragOverRight
        {
            get { return _isDragOverRight; }
            set { SetProperty(ref _isDragOverRight, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }
}
