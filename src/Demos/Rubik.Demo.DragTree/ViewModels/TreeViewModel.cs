using System.Collections.Generic;
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

        public TreeViewModelBase Parent { get; set; }
        public override IEnumerable<TreeViewModelBase> Nodes { get; set; } 
    }
}
