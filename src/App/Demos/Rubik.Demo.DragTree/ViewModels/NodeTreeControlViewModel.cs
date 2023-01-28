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
    public class NodeTreeControlViewModel : BindableBase
    {
        public DelegateCommand<RoutedEventArgs> LoadedCommand { get; private set; }

        private TreeView _treeView = null;
        private TreeDragUtil<TreeView, TreeViewItem> _treeDragUtil = null;

        public NodeTreeControlViewModel()
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
            _treeView = (e.OriginalSource as NodeTreeControl).TreeViewName;

            _treeDragUtil = new TreeDragUtil<TreeView, TreeViewItem>(_treeView, false);
            _treeDragUtil.OverlapAreaCheckHeight = 25;
            _treeDragUtil.DraggingTip = DraggingTip;
            _treeDragUtil.OnOverlapItem = OnOverlapFolder;
            _treeDragUtil.OnCompleted = OnCompleted;
        }

        private string DraggingTip(TreeViewItem item)
        {
            return (item.DataContext as TreeViewModel)?.Name;
        }

        private void OnOverlapFolder(OverlapArea overlapArea, TreeViewItem overlapItem, TreeViewItem draggingItem)
        {
            if (overlapArea == OverlapArea.Out)
            {
                TreeBackground = Brushes.Transparent;
                TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = false);
                return;
            }

            var overlapModel = overlapItem?.DataContext as TreeViewModel;
            var draggingModel = draggingItem.DataContext as TreeViewModel;

            if (draggingModel == null)
                return;

            /*
             * 1. 拖动项是目标目录时，不做移动
             * 2. 拖动项是目标目录的直接或间接父节点时，不做移动
             */
            if (overlapModel != null && draggingModel.Any<TreeViewModel>(m => m == overlapModel, true))
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
                overlapModel.IsExpanded = true;

                TreeViewModelBase.Foreach(TreeCollection, m =>
                {
                    m.IsDragOverUp = (overlapArea & OverlapArea.Up) == OverlapArea.Up;
                    m.IsDragOverDown = (overlapArea & OverlapArea.Down) == OverlapArea.Down;
                    m.IsDragOver = m == overlapModel;
                });
            }
        }

        private void OnCompleted(OverlapArea overlapArea, TreeViewItem overlapItem, TreeViewItem draggingItem)
        {
            TreeBackground = Brushes.Transparent;
            TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = false);

            if (overlapArea == OverlapArea.Out)
                return;

            var draggingModel = draggingItem.DataContext as TreeViewModel;
            var overlapModel = overlapItem?.DataContext as TreeViewModel;

            /*
             * 1. 拖动项是目标目录时，不做移动
             * 2. 拖动项是目标目录的直接或间接父节点时，不做移动
             */
            if (overlapModel != null && draggingModel.Any<TreeViewModel>(m => m == overlapModel, true))
            {
                TreeViewModelBase.Foreach(TreeCollection, m => m.IsDragOver = false);
                return;
            }

            Move(draggingModel, overlapModel, false, (overlapArea & OverlapArea.Up) == OverlapArea.Up);
        }

        /// <summary>
        /// 将源移动到目标节点的位置
        /// </summary>
        /// <param name="sourceModel">源节点</param>
        /// <param name="targetModel">目标节点，当移动到根目录时，该项为 null</param>
        /// <param name="keepSource">是否保留源节点</param>
        /// <param name="isTargetUp">是否添加到目标节点上方</param>
        private void Move(TreeViewModel sourceModel, TreeViewModel targetModel, bool keepSource = true, bool isTargetUp = true)
        {
            if (sourceModel == null || !keepSource && sourceModel == targetModel)
                return;

            var targetNodes = (ObservableCollection<TreeViewModel>)((targetModel?.Parent?.Nodes) ?? TreeCollection);
            var targetIndex = targetModel == null ? -1 : TreeViewModelBase.IndexOf(targetNodes, m => m == targetModel);

            //非根目录，源与目标处于同一父节点
            if (targetIndex >= 0 && targetModel.Parent == sourceModel.Parent)
            {
                var draggingIndex = TreeViewModelBase.IndexOf(targetNodes, m => m == sourceModel);
                var diff = targetIndex - draggingIndex;

                //源与目标是同一节点
                if (diff == 0)
                {
                    var newModel = sourceModel.Clone();
                    newModel.Parent = targetModel.Parent;

                    targetNodes.Insert(isTargetUp ? targetIndex : (targetIndex + 1), newModel);
                    return;
                }

                //相邻
                if (diff < 0)
                {
                    if (!keepSource)
                        targetNodes.Remove(sourceModel);

                    targetNodes.Insert(isTargetUp ? targetIndex : (targetIndex + 1), sourceModel);
                }
                else
                {
                    if (!keepSource)
                        targetNodes.Remove(sourceModel);

                    targetNodes.Insert(isTargetUp ? (targetIndex - 1) : targetIndex, sourceModel);
                }

                return;
            }

            if (!keepSource)
            {
                var sourceNodes = (ObservableCollection<TreeViewModel>)((sourceModel.Parent?.Nodes) ?? TreeCollection);
                sourceNodes.Remove(sourceModel);

                //添加到根目录
                if (targetIndex < 0)
                {
                    sourceModel.Parent = null;
                    targetNodes.Add(sourceModel);
                }
                else
                {
                    sourceModel.Parent = targetModel.Parent;
                    targetNodes.Insert(isTargetUp ? targetIndex : (targetIndex + 1), sourceModel);
                }
            }
            else
            {
                var newModel = sourceModel.Clone();

                //添加到根目录
                if (targetIndex < 0)
                {
                    newModel.Parent = null;
                    targetNodes.Add(newModel);
                }
                else
                {
                    newModel.Parent = targetModel.Parent;
                    targetNodes.Insert(isTargetUp ? targetIndex : (targetIndex + 1), newModel);
                }
            }
        }
    }
}
