using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;

using Rubik.Toolkit.Utils;
using Rubik.Toolkit.Adorners;
using Rubik.Toolkit.Datas;

namespace Rubik.Toolkit.UI
{
    /// <typeparam name="T">TreeView or MultiSelectTreeView</typeparam>
    /// <typeparam name="U">TreeViewItem or MultiSelectTreeViewItem</typeparam>
    public class TreeDragUtil<TContainer, TItem> where TContainer : ItemsControl where TItem : HeaderedItemsControl
    {
        //TreeView
        private TContainer _treeView = null;

        //TreeViewItem
        private TItem _draggingContainer = null;

        private AdornerLayer _adornerLayer = null;
        private MouseTipAdorner _panelAdorner = null;

        //Mouse Down
        private Point? _mouseDownPos = null;
        private TItem _overlapItem = null;
        private OverlapArea _overDragArea = OverlapArea.Out;

        public bool JustOverlapGroup { get; } = true;
        public double OverlapAreaCheckHeight { get; set; } = double.NaN;

        /// <summary>
        /// TItem : overlapItem
        /// 对于无子元素的项也有可能是组，取决于业务逻辑，此处提供自主判断入口
        /// </summary>
        public Func<TItem, bool> IsGroup { get; set; }

        /// <summary>
        /// isOverDragArea, overlapItem, draggingItem
        /// </summary>
        public Action<OverlapArea, TItem, TItem> OnCompleted { get; set; }

        /// <summary>
        /// isOverDragArea, overlapItem, draggingItem
        /// </summary>
        public Action<OverlapArea, TItem, TItem> OnOverlapItem { get; set; }

        public Func<TItem, string> DraggingTip { get; set; }

        public TreeDragUtil(TContainer treeView, bool justOverlapGroup = true, bool manualHandleMouseLeftButtonDown = false)
        {
            _treeView = treeView;
            JustOverlapGroup = justOverlapGroup;

            if (!manualHandleMouseLeftButtonDown)
            {
                _treeView.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
                _treeView.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            }

            _treeView.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
            _treeView.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
        }

        #region Public

        public void ManualHandleMouseLeftButtonDown(TItem treeViewItem, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left || e.ClickCount != 1)
                return;

            if (!_treeView.HasItems || e.OriginalSource.GetType().Name == "TextBoxView")
                return;

            _mouseDownPos = e.GetPosition(_treeView);
            Init(treeViewItem, e);
        }

        #endregion

        #region Event

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_treeView.HasItems || e.OriginalSource.GetType().Name == "TextBoxView")
                return;

            _mouseDownPos = e.GetPosition(_treeView);

            var result = VisualTreeHelper.HitTest(_treeView, _mouseDownPos.Value);
            if (result == null)
                return;

            var treeViewItem = TreeUtil.FindVisualParent<TItem>(result.VisualHit);
            if (treeViewItem == null)
                return;

            Init(treeViewItem, e);
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _treeView.PreviewMouseMove -= OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var curPos = e.GetPosition(_treeView);

            if (DoubleUtil.GreaterThanOrClose(Math.Abs(_mouseDownPos.Value.Y - curPos.Y), SystemParameters.MinimumVerticalDragDistance)
                || DoubleUtil.GreaterThanOrClose(Math.Abs(_mouseDownPos.Value.X - curPos.X), SystemParameters.MinimumHorizontalDragDistance))
            {
                Start();
            }
        }

        //Dragging
        private void OnQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            _panelAdorner.Update();
            Move();
        }

        #endregion

        #region Private

        private void Init(TItem treeViewItem, MouseButtonEventArgs e)
        {
            _draggingContainer = treeViewItem;

            _treeView.PreviewMouseMove -= OnMouseMove;
            _treeView.PreviewMouseMove += OnMouseMove;
        }

        private MouseTipAdorner ConstructMousePanelAdorner(UIElement panel, TItem draggingContainer)
        {
            if (panel == null || draggingContainer == null)
                return null;

            return new MouseTipAdorner(panel, DraggingTip(draggingContainer));
        }

        private MouseTipAdorner GetPanelAdorner(UIElement panel, TItem draggingContainer)
        {
            return ConstructMousePanelAdorner(panel, draggingContainer);
        }

        private void Start()
        {
            if (_panelAdorner != null || _draggingContainer == null)
                return;

            if (_adornerLayer == null)
                _adornerLayer = AdornerLayer.GetAdornerLayer(_treeView);

            if (_panelAdorner == null)
            {
                _panelAdorner = GetPanelAdorner(_treeView, _draggingContainer);
                _adornerLayer.Add(_panelAdorner);
            }

            DragDrop.AddQueryContinueDragHandler(_treeView, OnQueryContinueDrag);
            DragDrop.DoDragDrop(_treeView, _draggingContainer.DataContext, DragDropEffects.Move);
            DragDrop.RemoveQueryContinueDragHandler(_treeView, OnQueryContinueDrag);

            End();
        }

        private void End()
        {
            _treeView.PreviewMouseMove -= OnMouseMove;

            _adornerLayer.Remove(_panelAdorner);
            _panelAdorner = null;

            if (_draggingContainer != null)
                OnCompleted?.Invoke(_overDragArea, _overlapItem, _draggingContainer);

            _overDragArea = OverlapArea.Out;
            _overlapItem = null;
            _draggingContainer = null;
        }

        private void Move()
        {
            if ((DoubleUtil.IsZero(_draggingContainer.ActualWidth) || DoubleUtil.IsZero(_draggingContainer.ActualHeight))
                && _draggingContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return;

            var screenPos = new Win32.POINT();
            if (!Win32.GetCursorPos(ref screenPos))
                return;

            var draggingPoint = _treeView.PointFromScreen(new Point(screenPos.X, screenPos.Y));

            if (DoubleUtil.GreaterThan(draggingPoint.X, _treeView.ActualWidth) || DoubleUtil.LessThan(draggingPoint.X, 0)
                || DoubleUtil.LessThan(draggingPoint.Y, 0) || DoubleUtil.GreaterThan(draggingPoint.Y, _treeView.ActualHeight))
            {
                _overDragArea = OverlapArea.Out;
                OnOverlapItem?.Invoke(_overDragArea, _overlapItem, _draggingContainer);
                return;
            }

            _overlapItem = GetOverlapItem(_treeView, draggingPoint);
            OnOverlapItem?.Invoke(_overDragArea, _overlapItem, _draggingContainer);
        }

        private TItem GetOverlapItem(TContainer rootTreeView, Point draggingPoint)
        {
            //TODO X Offset
            var result = VisualTreeHelper.HitTest(rootTreeView, new Point(rootTreeView.ActualWidth - 30, draggingPoint.Y));
            if (result == null)
                return null;

            var overlapContainer = TreeUtil.FindVisualParent<TItem>(result.VisualHit);

            //overlapContainer 有可能是已经被折叠的子节点，故通过 IsVisible 来判断
            if (overlapContainer == null || !overlapContainer.IsVisible)
                return null;

            TItem overlapItem = null;

            if (JustOverlapGroup)
            {
                if (overlapContainer.HasItems || IsGroup != null && IsGroup(overlapContainer))
                    overlapItem = overlapContainer;
                else
                    overlapItem = ItemsControl.ItemsControlFromItemContainer(overlapContainer) as TItem;
            }
            else
                overlapItem = overlapContainer;

            if (overlapItem != null)
            {
                var overlapItemPos = overlapItem.TranslatePoint(new Point(), rootTreeView);
                var checkHeight = DoubleUtil.IsNaN(OverlapAreaCheckHeight) ? overlapItem.ActualHeight : OverlapAreaCheckHeight;

                if (DoubleUtil.GreaterThan(draggingPoint.Y, overlapItemPos.Y) && DoubleUtil.LessThanOrClose(draggingPoint.Y, overlapItemPos.Y + checkHeight / 2))
                {
                    _overDragArea = OverlapArea.Inner;
                    _overDragArea |= OverlapArea.Up;
                }

                if (DoubleUtil.GreaterThan(draggingPoint.Y, overlapItemPos.Y + checkHeight / 2) && DoubleUtil.LessThanOrClose(draggingPoint.Y, overlapItemPos.Y + checkHeight))
                {
                    _overDragArea = OverlapArea.Inner;
                    _overDragArea |= OverlapArea.Down;
                }
            }
            else
                _overDragArea = OverlapArea.Inner;

            return overlapItem;
        }

        #endregion
    }
}
