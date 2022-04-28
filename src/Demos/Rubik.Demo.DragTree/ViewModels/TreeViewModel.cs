using System.Linq;
using System.Collections.ObjectModel;

using Rubik.Toolkit.Datas;

namespace Rubik.Demo.DragTree.ViewModels
{
    public class TreeViewModel : TreeViewModelBase
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
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

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private bool _isNode;
        public bool IsNode
        {
            get { return _isNode; }
            set { SetProperty(ref _isNode, value); }
        }

        public TreeViewModel Clone()
        {
            var model = new TreeViewModel
            {
                Name = Name,
                IsNode = IsNode

            };

            if (Nodes != null && Nodes.Any())
                model.Nodes = new ObservableCollection<TreeViewModel>(Nodes.Select(n => ((TreeViewModel)n).Clone()));

            return model;
        }
    }
}
