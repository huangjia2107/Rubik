using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Prism.Mvvm;
using Prism.Commands;

using Rubik.Theme.UI;
using Rubik.Theme.Datas;
using Rubik.Theme.Utils;
using Rubik.Demo.DragTree.Views;

namespace Rubik.Demo.DragTree.ViewModels
{
    public class FileTreeControlViewModel : BindableBase
    {
        public DelegateCommand<RoutedEventArgs> LoadedCommand { get; private set; }

        private TreeView _treeView = null;
        private TreeDragUtil<TreeView, TreeViewItem> _treeDragUtil = null;

        public FileTreeControlViewModel()
        {
            LoadedCommand = new DelegateCommand<RoutedEventArgs>(Loaded);

            TreeCollection.Add(new TreeViewModel
            {
                Name = "FolderA",
                IsNode = true,
                Nodes = new ObservableCollection<TreeViewModel>
                {
                    new TreeViewModel
                    {
                        Name = "FileA.xml",
                        IsNode = false
                    },
                    new TreeViewModel
                    {
                        Name = "FileB.xml",
                        IsNode = false
                    }
                }
            });

            var folderB = new TreeViewModel
            {
                Name = "FolderB",
                IsNode = true
            };

            TreeCollection.Add(folderB);

            folderB.Nodes = new ObservableCollection<TreeViewModel>
            {
                new TreeViewModel
                {
                    Name = "FolderA",
                    IsNode = true,
                    Parent = folderB
                },
                new TreeViewModel
                {
                    Name = "FolderB",
                    IsNode = true,
                    Parent = folderB
                },
                new TreeViewModel
                {
                    Name = "FileA.xml",
                    IsNode = false,
                    Parent = folderB,
                },
                new TreeViewModel
                {
                    Name = "FileB.xml",
                    IsNode = false,
                    Parent = folderB,
                }
            };

            var folderB1 = ((ObservableCollection<TreeViewModel>)folderB.Nodes)[0];
            folderB1.Nodes = new ObservableCollection<TreeViewModel>
            {
                new TreeViewModel
                {
                    Name = "FileA.xml",
                    IsNode = false,
                    Parent = folderB1
                },
                new TreeViewModel
                {
                    Name = "FileB.xml",
                    IsNode = false,
                    Parent = folderB1
                }
            };

            var folderB2 = ((ObservableCollection<TreeViewModel>)folderB.Nodes)[1];
            folderB2.Nodes = new ObservableCollection<TreeViewModel>
            {
                new TreeViewModel
                {
                    Name = "FileA.xml",
                    IsNode = false,
                    Parent = folderB2
                },
                new TreeViewModel
                {
                    Name = "FileB.xml",
                    IsNode = false,
                    Parent = folderB2
                }
            };

            TreeCollection.AddRange(new List<TreeViewModel>
            {
                new TreeViewModel
                {
                    Name = "FolderC",
                    IsNode = true
                },
                new TreeViewModel
                {
                    Name = "FileA.xml",
                    IsNode = false
                },
                new TreeViewModel
                {
                    Name = "FileB.xml",
                    IsNode = false
                }
            });
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
            _treeView = (e.OriginalSource as FileTreeControl).TreeViewName;

            _treeDragUtil = new TreeDragUtil<TreeView, TreeViewItem>(_treeView, true);
            _treeDragUtil.IsGroup = IsNode;
            _treeDragUtil.DraggingTip = DraggingTip;
            _treeDragUtil.OnOverlapItem = OnOverlapItem;
            _treeDragUtil.OnCompleted = OnCompleted;
        }

        private bool IsNode(TreeViewItem item)
        {
            return (item.DataContext as TreeViewModel).IsNode;
        }

        private string DraggingTip(TreeViewItem item)
        {
            return (item.DataContext as TreeViewModel)?.Name;
        }

        private void OnOverlapItem(OverlapArea overlapArea, TreeViewItem overlapItem, TreeViewItem draggingItem)
        {
            if (overlapArea == OverlapArea.Out)
            {
                TreeBackground = Brushes.Transparent;
                TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = false);
                return;
            }

            var overlapFolderModel = overlapItem?.DataContext as TreeViewModel;
            var draggingModel = draggingItem.DataContext as TreeViewModel;

            /*
             * 1. 拖动项是目标目录的直接子节点时，包括目标目录是根目录的情况，不做移动
             *    注：目标目录是根目录时，overlapItem 是 null
             * 2. 拖动项是目标目录时，不做移动
             * 3. 拖动项是目标目录的直接或间接父节点时，不做移动
             */
            if (draggingModel.Parent == overlapFolderModel
                || draggingModel == overlapFolderModel
                || overlapFolderModel != null && draggingModel.Any<TreeViewModel>(m => m == overlapFolderModel))
            {
                TreeBackground = Brushes.Transparent;
                TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = false);
                return;
            }

            if (overlapFolderModel == null)
            {
                TreeBackground = ColorUtil.GetBrushFromString("#d6ebff");
                TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = false);
                return;
            }
            else
            {
                overlapFolderModel.IsExpanded = true;

                TreeBackground = Brushes.Transparent;
                TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = m == overlapFolderModel);
            }
        }

        private void OnCompleted(OverlapArea overlapArea, TreeViewItem overlapItem, TreeViewItem draggingItem)
        {
            TreeBackground = Brushes.Transparent;
            TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = false);

            if (overlapArea == OverlapArea.Out)
                return;

            var draggingModel = draggingItem.DataContext as TreeViewModel;
            var overlapFolderModel = overlapItem?.DataContext as TreeViewModel;

            /*
             * 1. 拖动项是目标目录的直接子节点时，包括目标目录是根目录的情况，不做移动
             *    注：目标目录是根目录时，overlapItem 是 null
             * 2. 拖动项是目标目录时，不做移动
             * 3. 拖动项是目标目录的直接或间接父节点时，不做移动
             */
            if (draggingModel.Parent == overlapFolderModel
                || draggingModel == overlapFolderModel
                || overlapFolderModel != null && draggingModel.Any<TreeViewModel>(m => m == overlapFolderModel))
                return;

            //TODO Move
        }
    }
}
