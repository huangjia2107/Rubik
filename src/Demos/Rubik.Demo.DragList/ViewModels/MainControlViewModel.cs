using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.ObjectModel;

using Prism.Mvvm;
using Prism.Commands;

using Rubik.Toolkit.Datas;
using Rubik.Toolkit.UI;
using Rubik.Toolkit.Utils;

using Rubik.Demo.DragList.Views;

namespace Rubik.Demo.DragList.ViewModels
{
    public class MainControlViewModel : BindableBase
    {
        public DelegateCommand<RoutedEventArgs> LoadedCommand { get; private set; }

        private ListBox _verListBox = null;
        private ListDragUtil<ListBox, ListBoxItem> _verListDragUtil = null;

        private ListBox _horListBox = null;
        private ListDragUtil<ListBox, ListBoxItem> _horListDragUtil = null;

        private ListBox _wrapListBox = null;
        private ListDragUtil<ListBox, ListBoxItem> _wrapListDragUtil = null;

        public MainControlViewModel()
        {
            LoadedCommand = new DelegateCommand<RoutedEventArgs>(Loaded);

            TreeCollection.Add(new TreeViewModel { Name = "FileA.xml" });
            TreeCollection.Add(new TreeViewModel { Name = "FileB.xml" });
            TreeCollection.Add(new TreeViewModel { Name = "FileC.xml" });
            TreeCollection.Add(new TreeViewModel { Name = "FileD.xml" });
            TreeCollection.Add(new TreeViewModel { Name = "FileE.xml" });
            TreeCollection.Add(new TreeViewModel { Name = "FileF.xml" });
            TreeCollection.Add(new TreeViewModel { Name = "FileG.xml" });
            TreeCollection.Add(new TreeViewModel { Name = "FileH.xml" });
            TreeCollection.Add(new TreeViewModel { Name = "FileI.xml" });
            TreeCollection.Add(new TreeViewModel { Name = "FileJ.xml" });
            TreeCollection.Add(new TreeViewModel { Name = "FileK.xml" });
        }

        public ObservableCollection<TreeViewModel> TreeCollection { get; } = new ObservableCollection<TreeViewModel>();

        private Brush _treeBackground = Brushes.Transparent;
        public Brush TreeBackground
        {
            get { return _treeBackground; }
            set { SetProperty(ref _treeBackground, value); }
        }

        private void Loaded(RoutedEventArgs e)
        {
            _verListBox = (e.OriginalSource as MainControl).VerListBoxName;
            _verListDragUtil = new ListDragUtil<ListBox, ListBoxItem>(_verListBox, false, true, true);

            _horListBox = (e.OriginalSource as MainControl).HorListBoxName;
            _horListDragUtil = new ListDragUtil<ListBox, ListBoxItem>(_horListBox, false, false);

            _wrapListBox = (e.OriginalSource as MainControl).WrapListBoxName;
            _wrapListDragUtil = new ListDragUtil<ListBox, ListBoxItem>(_wrapListBox, false, true, false);

            _verListDragUtil.DraggingTip = DraggingTip;
            _horListDragUtil.DraggingTip = DraggingTip;
            _wrapListDragUtil.DraggingTip = DraggingTip;

            _verListDragUtil.OnOverlapItem = OnOverlapItem;
            _horListDragUtil.OnOverlapItem = OnOverlapItem;
            _wrapListDragUtil.OnOverlapItem = OnOverlapItem;

            _verListDragUtil.OnCompleted = OnCompleted;
            _horListDragUtil.OnCompleted = OnCompleted;
            _wrapListDragUtil.OnCompleted = OnCompleted;
        }

        private string DraggingTip(ListBoxItem item)
        {
            return (item.DataContext as TreeViewModel)?.Name;
        }

        private void OnOverlapItem(OverlapArea overlapArea, ListBoxItem overlapItem, ListBoxItem draggingItem)
        {
            var draggingModel = draggingItem.DataContext as TreeViewModel;
            draggingModel.IsDragSource = true;

            if (overlapArea == OverlapArea.Out)
            {
                TreeBackground = Brushes.Transparent;
                TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = false);
                return;
            }

            var overlapModel = overlapItem?.DataContext as TreeViewModel;

            /*
             * 1. 拖动项是目标目录时，不做移动
             */
            if (overlapModel != null && overlapModel == draggingModel)
            {
                TreeBackground = Brushes.Transparent;
                TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = false);
                return;
            }

            //根目录
            if (overlapModel == null)
            {
                TreeBackground = ColorUtil.GetBrushFromString("#d6ebff");
                TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = false);
            }
            else
            {
                TreeBackground = Brushes.Transparent;

                TreeViewModelBase.Foreach(TreeCollection, m =>
                {
                    m.IsDragOverUp = (overlapArea & OverlapArea.Up) == OverlapArea.Up;
                    m.IsDragOverDown = (overlapArea & OverlapArea.Down) == OverlapArea.Down;
                    m.IsDragOverLeft = (overlapArea & OverlapArea.Left) == OverlapArea.Left;
                    m.IsDragOverRight = (overlapArea & OverlapArea.Right) == OverlapArea.Right;
                    m.IsDragOver = m == overlapModel;
                });
            }
        }

        private void OnCompleted(OverlapArea overlapArea, ListBoxItem overlapItem, ListBoxItem draggingItem)
        {
            TreeBackground = Brushes.Transparent;
            TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = m.IsDragSource = false);

            if (overlapArea == OverlapArea.Out)
                return;

            var draggingModel = draggingItem.DataContext as TreeViewModel;
            var overlapModel = overlapItem?.DataContext as TreeViewModel;

            /*
             * 1. 拖动项是目标目录时，不做移动
             */
            if (overlapModel != null && overlapModel == draggingModel)
            {
                TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = false);
                return;
            }

            //Move(draggingModel, overlapModel, false, (overlapArea & OverlapArea.Up) == OverlapArea.Up);
        }
    }

}
