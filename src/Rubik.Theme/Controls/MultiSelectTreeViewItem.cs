using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Collections;

namespace Rubik.Theme.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MultiSelectTreeViewItem))]
    [TemplatePart(Name = HeaderPartName, Type = typeof(FrameworkElement))]
    public class MultiSelectTreeViewItem : HeaderedItemsControl
    {
        private const string HeaderPartName = "PART_Header";
        private FrameworkElement _headerElement = null;

        private bool CanExpand
        {
            get { return HasItems; }
        }

        private bool CanExpandOnInput
        {
            get { return !this.CanExpand ? false : IsEnabled; }
        }

        private ItemsControl ParentItemsControl
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this); }
        }

        private MultiSelectTreeView ParentTreeView
        {
            get
            {
                for (var i = ParentItemsControl; i != null; i = ItemsControl.ItemsControlFromItemContainer(i))
                {
                    var treeView = i as MultiSelectTreeView;
                    if (treeView != null)
                    {
                        return treeView;
                    }
                }
                return null;
            }
        }

        private MultiSelectTreeViewItem ParentTreeViewItem
        {
            get { return ParentItemsControl as MultiSelectTreeViewItem; }
        }

        static MultiSelectTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectTreeViewItem), new FrameworkPropertyMetadata(typeof(MultiSelectTreeViewItem)));
            IsTabStopProperty.OverrideMetadata(typeof(MultiSelectTreeViewItem), new FrameworkPropertyMetadata(false));

            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(MultiSelectTreeViewItem), new FrameworkPropertyMetadata((object)KeyboardNavigationMode.Continue));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(MultiSelectTreeViewItem), new FrameworkPropertyMetadata((object)KeyboardNavigationMode.None));

            EventManager.RegisterClassHandler(typeof(MultiSelectTreeViewItem), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(OnRequestBringIntoView));
        }

        #region Event

        public readonly static RoutedEvent ExpandedEvent = EventManager.RegisterRoutedEvent("Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MultiSelectTreeViewItem));
        public event RoutedEventHandler Expanded
        {
            add { AddHandler(MultiSelectTreeViewItem.ExpandedEvent, value); }
            remove { RemoveHandler(MultiSelectTreeViewItem.ExpandedEvent, value); }
        }

        public readonly static RoutedEvent CollapsedEvent = EventManager.RegisterRoutedEvent("Collapsed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MultiSelectTreeViewItem));
        public event RoutedEventHandler Collapsed
        {
            add { AddHandler(MultiSelectTreeViewItem.CollapsedEvent, value); }
            remove { RemoveHandler(MultiSelectTreeViewItem.CollapsedEvent, value); }
        }

        public readonly static RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MultiSelectTreeViewItem));
        public event RoutedEventHandler Selected
        {
            add { AddHandler(MultiSelectTreeViewItem.SelectedEvent, value); }
            remove { RemoveHandler(MultiSelectTreeViewItem.SelectedEvent, value); }
        }

        public readonly static RoutedEvent UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MultiSelectTreeViewItem));
        public event RoutedEventHandler Unselected
        {
            add { AddHandler(MultiSelectTreeViewItem.UnselectedEvent, value); }
            remove { RemoveHandler(MultiSelectTreeViewItem.UnselectedEvent, value); }
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(MultiSelectTreeViewItem), new FrameworkPropertyMetadata(false, OnIsExpandedChanged));
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeViewItem = (MultiSelectTreeViewItem)d;
            var newValue = (bool)e.NewValue;

            if (newValue)
            {
                treeViewItem.OnExpanded(new RoutedEventArgs(MultiSelectTreeViewItem.ExpandedEvent, treeViewItem));
                return;
            }

            if (treeViewItem.ContainSelection())
            {
                if (treeViewItem.IsKeyboardFocused && Keyboard.FocusedElement == treeViewItem)
                    treeViewItem.Select(true, SelectionMode.Single);
                else
                    treeViewItem.Focus();
            }

            treeViewItem.OnCollapsed(new RoutedEventArgs(MultiSelectTreeViewItem.CollapsedEvent, treeViewItem));
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(MultiSelectTreeViewItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsSelectedChanged));
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeViewItem = (MultiSelectTreeViewItem)d;
            var newValue = (bool)e.NewValue;

            treeViewItem.Select(newValue, SelectionMode.Multiple);

            if (newValue)
            {
                treeViewItem.OnSelected(new RoutedEventArgs(MultiSelectTreeViewItem.SelectedEvent, treeViewItem));
                return;
            }

            treeViewItem.OnUnselected(new RoutedEventArgs(MultiSelectTreeViewItem.UnselectedEvent, treeViewItem));
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _headerElement = GetTemplateChild("PART_Header") as FrameworkElement;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MultiSelectTreeViewItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MultiSelectTreeViewItem();
        }

        //called before a container is used
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        //called when a container is thrown away or recycled
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

            var container = element as MultiSelectTreeViewItem;
            var parentTreeView = ParentTreeView;

            if (container != null && parentTreeView != null)
                parentTreeView.RemoveSelectedElement(container, item, true);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!e.Handled && IsEnabled)
            {
                if (e.ClickCount % 2 == 0)
                    IsExpanded = !IsExpanded;

                if (Keyboard.FocusedElement == this && this.IsKeyboardFocused)
                    MouseLeftButtonDownSelect();

                Focus();
                e.Handled = true;
            }

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            if (!e.Handled && IsEnabled)
            {
                var treeView = ParentTreeView;
                if (treeView != null && treeView.SelectionMode == SelectionMode.Multiple && Keyboard.FocusedElement == this && this.IsKeyboardFocused)
                    Select(!IsSelected, GetSelectionMode());

                Focus();
                e.Handled = true;
            }

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                var treeView = ParentTreeView;
                if (treeView != null && treeView.SelectionMode == SelectionMode.Multiple)
                {
                    Select(!IsSelected, SelectionMode.Multiple);
                }
                else if (treeView.SelectedItems == null || treeView.SelectedItems.Count <= 1 || !IsSelected)
                {
                    Select(true, SelectionMode.Single);
                }
            }
            else
                MouseLeftButtonDownSelect();

            base.OnGotFocus(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            var treeView = ParentTreeView;
            if (!e.Handled && treeView != null && treeView.SelectionMode != SelectionMode.Multiple)
            {
                var key = e.Key;
                switch (key)
                {
                    case Key.Left:
                    case Key.Right:
                        {
                            if (!this.LogicalLeft(e.Key))
                            {
                                if (IsControlKeyDown || !CanExpandOnInput || !_headerElement.IsVisible)
                                    break;

                                if (!IsExpanded)
                                {
                                    IsExpanded = true;

                                    e.Handled = true;
                                    return;
                                }

                                if (!HandleDownKey())
                                    break;

                                e.Handled = true;
                                return;
                            }
                            else
                            {
                                if (IsControlKeyDown || !CanExpandOnInput || !IsExpanded || !_headerElement.IsVisible)
                                    break;

                                if (!IsFocused)
                                    Focus();
                                else
                                    IsExpanded = false;

                                e.Handled = true;
                                return;
                            }
                        }
                    case Key.Up:
                        {
                            if (IsControlKeyDown || !HandleUpKey())
                                break;

                            e.Handled = true;
                            break;
                        }
                    case Key.Down:
                        {
                            if (IsControlKeyDown || !HandleDownKey())
                                break;

                            e.Handled = true;
                            return;
                        }
                    case Key.Home:
                        {
                            if (IsControlKeyDown || !HandleHomeKey())
                                break;

                            e.Handled = true;
                            return;
                        }
                    case Key.End:
                        {
                            if (IsControlKeyDown || !HandleEndKey())
                                break;

                            e.Handled = true;
                            return;
                        }
                    default:
                        {
                            switch (key)
                            {
                                case Key.Add:
                                    {
                                        if (!this.CanExpandOnInput || this.IsExpanded)
                                            return;

                                        this.IsExpanded = true;
                                        e.Handled = true;
                                        return;
                                    }
                                case Key.Separator:
                                    {
                                        break;
                                    }
                                case Key.Subtract:
                                    {
                                        if (!this.CanExpandOnInput || !this.IsExpanded)
                                            return;

                                        this.IsExpanded = false;
                                        e.Handled = true;
                                        return;
                                    }
                                default:
                                    {
                                        return;
                                    }
                            }
                            break;
                        }
                }
            }
        }

        #endregion

        #region Static

        private static bool IsControlKeyDown
        {
            get { return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control; }
        }

        private static bool IsShiftKeyDown
        {
            get { return (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift; }
        }

        private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            if (e.TargetObject == sender)
            {
                ((MultiSelectTreeViewItem)sender).HandleBringIntoView(e);
            }
        }

        private static MultiSelectTreeViewItem FindLastFocusableItem(MultiSelectTreeViewItem item)
        {
            MultiSelectTreeViewItem treeViewItem = null;
            int count = -1;
            MultiSelectTreeViewItem treeViewItem1 = null;

            while (item != null)
            {
                if (!item.IsEnabled || !item.IsVisible)
                {
                    if (count <= 0)
                        break;

                    count--;
                }
                else
                {
                    if (!item.IsExpanded || !item.CanExpand)
                        return item;

                    treeViewItem = item;
                    treeViewItem1 = item;
                    count = item.Items.Count - 1;
                }

                item = treeViewItem1.ItemContainerGenerator.ContainerFromIndex(count) as MultiSelectTreeViewItem;
            }

            if (treeViewItem != null)
                return treeViewItem;

            return null;
        }

        internal static bool FocusIntoItem(MultiSelectTreeViewItem item)
        {
            var treeViewItem = FindLastFocusableItem(item);
            if (treeViewItem == null)
                return false;

            return treeViewItem.Focus();
        }

        #endregion

        #region Handle Key Func

        internal bool FocusDown()
        {
            var treeViewItem = FindNextFocusableItem(true);
            if (treeViewItem == null)
                return true;

            return treeViewItem.Focus();
        }

        private bool HandleDownKey()
        {
            return FocusDown();
        }

        private bool HandleUpKey()
        {
            var itemsControl = FindPreviousFocusableItem();
            if (itemsControl != null)
            {
                if (itemsControl == ParentItemsControl && itemsControl == ParentTreeView)
                    return true;

                return itemsControl.Focus();
            }

            return false;
        }

        private bool HandleHomeKey()
        {
            var parentTreeView = ParentTreeView;
            if (parentTreeView == null)
                return false;

            return parentTreeView.FocusFirstItem();
        }

        private bool HandleEndKey()
        {
            var parentTreeView = ParentTreeView;
            if (parentTreeView == null)
                return false;

            return parentTreeView.FocusLastItem();
        }

        private MultiSelectTreeViewItem FindNextFocusableItem(bool walkIntoSubtree)
        {
            if (walkIntoSubtree && CanExpand && IsExpanded)
            {
                var treeViewItem1 = ItemContainerGenerator.ContainerFromIndex(0) as MultiSelectTreeViewItem;
                if (treeViewItem1 != null)
                {
                    if (treeViewItem1.IsEnabled && treeViewItem1._headerElement.IsVisible)
                        return treeViewItem1;

                    return treeViewItem1.FindNextFocusableItem(false);
                }
            }

            var parentItemsControl = ParentItemsControl;
            if (parentItemsControl != null)
            {
                int num = parentItemsControl.ItemContainerGenerator.IndexFromContainer(this);
                int count = parentItemsControl.Items.Count;

                MultiSelectTreeViewItem treeViewItem;
                while (num < count)
                {
                    num++;

                    treeViewItem = parentItemsControl.ItemContainerGenerator.ContainerFromIndex(num) as MultiSelectTreeViewItem;
                    if (treeViewItem == null || !treeViewItem.IsEnabled || !treeViewItem.IsVisible)
                        continue;

                    if (!treeViewItem._headerElement.IsVisible)
                        return treeViewItem.FindNextFocusableItem(true);

                    return treeViewItem;
                }

                treeViewItem = parentItemsControl as MultiSelectTreeViewItem;
                if (treeViewItem != null)
                {
                    return treeViewItem.FindNextFocusableItem(false);
                }
            }

            return null;
        }

        private ItemsControl FindPreviousFocusableItem()
        {
            var parentItemsControl = ParentItemsControl;
            if (parentItemsControl == null)
                return null;

            int num = parentItemsControl.ItemContainerGenerator.IndexFromContainer(this);
            if (num == 0)
            {
                var parentTreeViewItem = ParentTreeViewItem;
                if (parentTreeViewItem != null && !parentTreeViewItem._headerElement.IsVisible)
                    return parentTreeViewItem.FindPreviousFocusableItem();
            }

            while (num > 0)
            {
                num--;

                var treeViewItem = parentItemsControl.ItemContainerGenerator.ContainerFromIndex(num) as MultiSelectTreeViewItem;
                if (treeViewItem == null || !treeViewItem.IsEnabled || !treeViewItem.IsVisible)
                    continue;

                var treeViewItem1 = FindLastFocusableItem(treeViewItem);
                if (treeViewItem1 == null)
                    continue;

                return treeViewItem1;
            }

            return parentItemsControl;
        }

        private bool LogicalLeft(Key key)
        {
            bool flowDirection = (FlowDirection == FlowDirection.RightToLeft);
            if (!flowDirection && key == Key.Left)
                return true;

            if (!flowDirection)
                return false;

            return key == Key.Right;
        }

        #endregion

        #region Func

        private void MouseLeftButtonDownSelect()
        {
            var treeView = ParentTreeView;
            Select(treeView == null ? IsControlKeyDown : (IsControlKeyDown || treeView.SelectionMode == SelectionMode.Multiple) ? (!IsSelected) : true, GetSelectionMode());
        }

        private bool ContainSelection()
        {
            if (!HasItems)
                return false;

            for (int i = 0; i < Items.Count; i++)
            {
                var container = ItemContainerGenerator.ContainerFromIndex(i) as MultiSelectTreeViewItem;
                if (container != null)
                {
                    if (container.IsSelected || container.ContainSelection())
                        return true;
                }
            }

            return false;
        }

        private SelectionMode GetSelectionMode()
        {
            if (IsControlKeyDown)
                return SelectionMode.Multiple;

            if (IsShiftKeyDown)
                return SelectionMode.Extended;

            return SelectionMode.Single;
        }

        private void RemoveSelectedElementsWithInvalidContainer(IList oldItems, bool isUpdateSelection)
        {
            var parentTreeView = ParentTreeView;
            if (parentTreeView == null || !HasItems && (oldItems == null || oldItems.Count == 0))
                return;

            parentTreeView.RemoveSelectedElementsWithInvalidContainer(isUpdateSelection);
        }

        private object GetItemOrContainerFromContainer(ItemsControl itemsControl, DependencyObject container)
        {
            object obj = itemsControl.ItemContainerGenerator.ItemFromContainer(container);
            if (obj == DependencyProperty.UnsetValue && ItemsControl.ItemsControlFromItemContainer(container) == this && container is UIElement)
            {
                obj = container;
            }
            return obj;
        }

        private void Select(bool selected, SelectionMode selectionMode)
        {
            var parentTreeView = ParentTreeView;
            var parentItemsControl = ParentItemsControl;

            if (parentTreeView != null && parentItemsControl != null && !parentTreeView.IsChangingSelection)
            {
                parentTreeView.ChangeSelection(GetItemOrContainerFromContainer(parentItemsControl, this), this, selected, selectionMode);
            }
        }

        protected virtual void OnSelected(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        protected virtual void OnUnselected(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        protected virtual void OnCollapsed(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        protected virtual void OnExpanded(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        private void HandleBringIntoView(RequestBringIntoViewEventArgs e)
        {
            for (var i = ParentTreeViewItem; i != null; i = i.ParentTreeViewItem)
            {
                if (!i.IsExpanded)
                    i.IsExpanded = true;
            }

            if (e.TargetRect.IsEmpty)
            {
                var headerElement = GetFirstHeaderElement();
                if (headerElement != null)
                {
                    headerElement.BringIntoView();
                    e.Handled = true;
                }
            }
        }

        internal FrameworkElement GetHeaderElement()
        {
            return _headerElement;
        }

        private FrameworkElement GetFirstHeaderElement()
        {
            var headerElement = _headerElement;
            if (headerElement != null)
            {
                if (headerElement.IsVisible)
                    return headerElement;

                if (HasItems)
                {
                    var count = Items.Count;
                    var num = 0;

                    while (num < count)
                    {
                        var treeViewItem = ItemContainerGenerator.ContainerFromIndex(num) as MultiSelectTreeViewItem;
                        if (treeViewItem == null || !treeViewItem.IsVisible)
                        {
                            num++;
                            continue;
                        }

                        return treeViewItem.GetFirstHeaderElement();
                    }
                }
            }

            return null;
        }

        private DependencyObject InternalPredictFocus(FocusNavigationDirection direction)
        {
            switch (direction)
            {
                case FocusNavigationDirection.Left:
                case FocusNavigationDirection.Up:
                    {
                        return FindPreviousFocusableItem();
                    }
                case FocusNavigationDirection.Right:
                case FocusNavigationDirection.Down:
                    {
                        return FindNextFocusableItem(true);
                    }
            }

            return null;
        }

        public int InDepth()
        {
            int i = 1;
            var parent = ItemsControl.ItemsControlFromItemContainer(this);

            while (parent is MultiSelectTreeViewItem)
            {
                i++;
                parent = ItemsControl.ItemsControlFromItemContainer(parent);
            }

            return i;
        }

        private void SubItemsMaxDepth(ref int subtreeDepth)
        {
            if (!HasItems)
                subtreeDepth = 0;
            else
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    var curMaxDepth = 0;
                    var container = ItemContainerGenerator.ContainerFromIndex(i) as MultiSelectTreeViewItem;

                    container.SubItemsMaxDepth(ref curMaxDepth);

                    subtreeDepth = Math.Max(subtreeDepth, curMaxDepth);
                }

                subtreeDepth++;
            }
        }

        public int ContainDepth()
        {
            if (!HasItems)
                return 0;
            else
            {
                var curSubtreeDepth = 0;

                SubItemsMaxDepth(ref curSubtreeDepth);
                return curSubtreeDepth;
            }
        }

        #endregion
    }
}