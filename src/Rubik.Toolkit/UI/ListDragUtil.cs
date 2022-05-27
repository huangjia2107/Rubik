using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;

using Rubik.Toolkit.Utils;
using Rubik.Toolkit.Adorners;
using Rubik.Toolkit.Datas;

namespace Rubik.Toolkit.UI
{
    public class ListDragUtil<TContainer, TItem> where TContainer : ItemsControl where TItem : FrameworkElement
    {
        //ListBox
        private TContainer _itemsControl = null;

        //ListBoxItem
        private TItem _draggingContainer = null;

        private AdornerLayer _adornerLayer = null;
        private MouseTipAdorner _panelAdorner = null;

        //Mouse Down
        private Point? _mouseDownPos = null;
        private TItem _overlapItem = null;
        private OverlapArea _overDragArea = OverlapArea.Out;

        private bool _dragingWithModifierKeys = false;
        private bool _handledWhileWithModifierKeys = false;
        private ModifierKeys _drggingModifierKeys = ModifierKeys.Control;

        /// <summary>
        /// isOverDragArea, overlapItem, draggingItem
        /// </summary>
        public Action<OverlapArea, TItem, TItem> OnCompleted { get; set; }

        /// <summary>
        /// isOverDragArea, overlapItem, draggingItem
        /// </summary>
        public Action<OverlapArea, TItem, TItem> OnOverlapItem { get; set; }

        public Func<TItem, string> DraggingTip { get; set; }

        public ListDragUtil(TContainer treeView,
            bool manualHandleMouseLeftButtonDown = false,
            bool dragingWithModifierKeys = false,
            bool handledWhileWithModifierKeys = false,
            ModifierKeys drggingModifierKeys = ModifierKeys.Control)
        {
            _itemsControl = treeView;

            _dragingWithModifierKeys = dragingWithModifierKeys;
            _handledWhileWithModifierKeys = handledWhileWithModifierKeys;
            _drggingModifierKeys = drggingModifierKeys;

            if (!manualHandleMouseLeftButtonDown)
            {
                _itemsControl.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
                _itemsControl.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            }

            _itemsControl.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
            _itemsControl.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
        }

        #region Public

        public void ManualHandleMouseLeftButtonDown(TItem treeViewItem, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left || e.ClickCount != 1)
                return;

            if (!_itemsControl.HasItems || e.OriginalSource.GetType().Name == "TextBoxView")
                return;

            _mouseDownPos = e.GetPosition(_itemsControl);
            Init(treeViewItem, e);
        }

        #endregion

        #region Event

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_itemsControl.HasItems || e.OriginalSource.GetType().Name == "TextBoxView")
                return;

            _mouseDownPos = e.GetPosition(_itemsControl);

            var result = VisualTreeHelper.HitTest(_itemsControl, _mouseDownPos.Value);
            if (result == null)
                return;

            var treeViewItem = TreeUtil.FindVisualParent<TItem>(result.VisualHit);
            if (treeViewItem == null)
                return;

            Init(treeViewItem, e);
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _itemsControl.PreviewMouseMove -= OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var curPos = e.GetPosition(_itemsControl);

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
            if (_dragingWithModifierKeys && (Keyboard.Modifiers & _drggingModifierKeys) != _drggingModifierKeys)
                return;

            _draggingContainer = treeViewItem;

            _itemsControl.PreviewMouseMove -= OnMouseMove;
            _itemsControl.PreviewMouseMove += OnMouseMove;

            if (_dragingWithModifierKeys && _handledWhileWithModifierKeys)
                e.Handled = true;
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
                _adornerLayer = AdornerLayer.GetAdornerLayer(_itemsControl);

            if (_panelAdorner == null)
            {
                _panelAdorner = GetPanelAdorner(_itemsControl, _draggingContainer);
                _adornerLayer.Add(_panelAdorner);
            }

            DragDrop.AddQueryContinueDragHandler(_itemsControl, OnQueryContinueDrag);
            DragDrop.DoDragDrop(_itemsControl, _draggingContainer.DataContext, DragDropEffects.Move);
            DragDrop.RemoveQueryContinueDragHandler(_itemsControl, OnQueryContinueDrag);

            End();
        }

        private void End()
        {
            _itemsControl.PreviewMouseMove -= OnMouseMove;

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
            if (DoubleUtil.IsZero(_draggingContainer.ActualWidth) || DoubleUtil.IsZero(_draggingContainer.ActualHeight))
                return;

            var screenPos = new Win32.POINT();
            if (!Win32.GetCursorPos(ref screenPos))
                return;

            var draggingPoint = _itemsControl.PointFromScreen(new Point(screenPos.X, screenPos.Y));

            if (DoubleUtil.GreaterThan(draggingPoint.X, _itemsControl.ActualWidth) || DoubleUtil.LessThan(draggingPoint.X, 0)
                || DoubleUtil.LessThan(draggingPoint.Y, 0) || DoubleUtil.GreaterThan(draggingPoint.Y, _itemsControl.ActualHeight))
            {
                _overDragArea = OverlapArea.Out;
                OnOverlapItem?.Invoke(_overDragArea, _overlapItem, _draggingContainer);
                return;
            }

            _overlapItem = GetOverlapItem(_itemsControl, draggingPoint);
            OnOverlapItem?.Invoke(_overDragArea, _overlapItem, _draggingContainer);
        }

        private TItem GetOverlapItem(TContainer rootTreeView, Point draggingPoint)
        {
            //TODO X Offset
            var result = VisualTreeHelper.HitTest(rootTreeView, draggingPoint);
            if (result == null)
                return null;

            var overlapContainer = TreeUtil.FindVisualParent<TItem>(result.VisualHit);

            //overlapContainer 有可能是已经被折叠的子节点，故通过 IsVisible 来判断
            if (overlapContainer == null || !overlapContainer.IsVisible)
                return null;

            _overDragArea = OverlapArea.Inner;

            if (overlapContainer != null)
            {
                var overlapItemPos = overlapContainer.TranslatePoint(new Point(), rootTreeView);

                if (DoubleUtil.LessThanOrClose(draggingPoint.Y, overlapItemPos.Y + overlapContainer.ActualHeight / 2))
                    _overDragArea |= OverlapArea.Up;
                else
                    _overDragArea |= OverlapArea.Down;

                if (DoubleUtil.LessThanOrClose(draggingPoint.X, overlapItemPos.X + overlapContainer.ActualWidth / 2))
                    _overDragArea |= OverlapArea.Left;
                else
                    _overDragArea |= OverlapArea.Right;
            }

            return overlapContainer;
        }

        #endregion
    }
}
