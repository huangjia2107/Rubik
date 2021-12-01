using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Collections;
using System.Windows.Input;
using System.Windows.Data;
using System.Xml;

using Rubik.Theme.Utils;

namespace Rubik.Theme.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MultiSelectTreeViewItem))]
    [TemplatePart(Name = ScrollHostPartName, Type = typeof(ScrollViewer))]
    public class MultiSelectTreeView : ItemsControl
    {
        class ContainerItemPair
        {
            public MultiSelectTreeViewItem Container { get; set; }
            public object Item { get; set; }

            public ContainerItemPair(MultiSelectTreeViewItem container, object item)
            {
                Container = container;
                Item = item;
            }
        }

        private const string ScrollHostPartName = "PART_ScrollHost";
        private ScrollViewer _scrollHost = null;

        private Binding _selectedValueBinding = null;

        private readonly List<ContainerItemPair> _selectedElements = null;
        private MultiSelectTreeViewItem _latestSelectedContainer = null;

        internal bool IsChangingSelection { get; private set; }

        #region Constructors

        static MultiSelectTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectTreeView), new FrameworkPropertyMetadata(typeof(MultiSelectTreeView)));

            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(MultiSelectTreeView), new FrameworkPropertyMetadata((object)KeyboardNavigationMode.Contained));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(MultiSelectTreeView), new FrameworkPropertyMetadata((object)KeyboardNavigationMode.None));
        }

        public MultiSelectTreeView()
        {
            _selectedElements = new List<ContainerItemPair>();
        }

        #endregion

        #region Events

        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(MultiSelectTreeView));
        public event RoutedPropertyChangedEventHandler<object> SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }

        protected virtual void OnSelectionChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            RaiseEvent(e);
        }

        #endregion

        #region Properties

        //SelectionMode
        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(MultiSelectTreeView), new UIPropertyMetadata(SelectionMode.Extended));
        public SelectionMode SelectionMode
        {
            get { return (SelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        //SelectedItem
        private readonly static DependencyPropertyKey SelectedItemPropertyKey =
            DependencyProperty.RegisterReadOnly("SelectedItem", typeof(object), typeof(MultiSelectTreeView), new FrameworkPropertyMetadata(null));

        public readonly static DependencyProperty SelectedItemProperty = SelectedItemPropertyKey.DependencyProperty;
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
        }

        //SelectedItems
        private static readonly DependencyPropertyKey SelectedItemsPropertyKey =
            DependencyProperty.RegisterReadOnly("SelectedItems", typeof(IList), typeof(MultiSelectTreeView), new FrameworkPropertyMetadata((IList)null));

        private static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;
        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
        }

        //SetSelectedItems
        private void SetSelectedItems()
        {
            var newSelectedItem = (_selectedElements == null || _selectedElements.Count == 0) ? null : _selectedElements.Last().Item;
            var oldSelectedItem = (SelectedItems == null || SelectedItems.Count == 0) ? null : SelectedItems[SelectedItems.Count - 1];

            SetValue(SelectedItemsPropertyKey, _selectedElements.Select(p => p.Item).ToList());

            if (oldSelectedItem != newSelectedItem)
            {
                SetValue(SelectedItemPropertyKey, newSelectedItem);
                UpdateSelectedValue(newSelectedItem);

                OnSelectionChanged(new RoutedPropertyChangedEventArgs<object>(oldSelectedItem, newSelectedItem, SelectionChangedEvent));
            }
        }

        //SelectedValue
        private readonly static DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(object), typeof(MultiSelectTreeView));
        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            private set { SetValue(SelectedValueProperty, value); }
        }

        //SelectedValuePath
        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(MultiSelectTreeView), new FrameworkPropertyMetadata(string.Empty, OnSelectedValuePathChanged));
        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        static void OnSelectedValuePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeView = (MultiSelectTreeView)d;

            BindingOperations.ClearBinding(treeView, SelectedValuePathProperty);
            treeView.UpdateSelectedValue(treeView.SelectedItem);
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _scrollHost = GetTemplateChild(ScrollHostPartName) as ScrollViewer;
        }

        //check whether item is its own container before creating container
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MultiSelectTreeViewItem;
        }

        //create container for every item in Items to show on UI
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
            if (container != null)
                RemoveSelectedElement(container, item, true);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var handled = false;
            var key = e.Key;

            switch (key)
            {
                case Key.Divide:
                case Key.Oem2:
                case Key.A:
                    // Ctrl-Fowardslash = Select All
                    if (((Keyboard.Modifiers & ModifierKeys.Control) == (ModifierKeys.Control)) && (SelectionMode == SelectionMode.Extended))
                    {
                        SelectAll();
                        handled = true;
                    }

                    break;
            }

            if (handled)
                e.Handled = true;
            else
                base.OnKeyDown(e);
        }

        #endregion

        #region Focus Func

        internal bool FocusFirstItem()
        {
            var treeViewItem = ItemContainerGenerator.ContainerFromIndex(0) as MultiSelectTreeViewItem;
            if (treeViewItem == null)
                return false;

            if (treeViewItem.IsEnabled && treeViewItem.IsVisible && treeViewItem.Focus())
                return true;

            return treeViewItem.FocusDown();
        }

        internal bool FocusLastItem()
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                var treeViewItem = ItemContainerGenerator.ContainerFromIndex(i) as MultiSelectTreeViewItem;
                if (treeViewItem != null && treeViewItem.IsEnabled && treeViewItem.IsVisible)
                    return MultiSelectTreeViewItem.FocusIntoItem(treeViewItem);
            }

            return false;
        }

        #endregion

        #region Selection

        public void SelectAll()
        {
            if (SelectionMode == SelectionMode.Extended && !IsChangingSelection)
            {
                IsChangingSelection = true;
                _selectedElements.Clear();

                ForearchTreeViewItem(this, null, true,
                  (container, item) =>
                  {
                      if (!container.IsSelected)
                          container.IsSelected = true;

                      _selectedElements.Add(new ContainerItemPair(container, item));

                  });

                SetSelectedItems();
                IsChangingSelection = false;
            }
        }

        public void UnselectAll()
        {
            if (_selectedElements.Count > 0)
            {
                IsChangingSelection = true;

                _selectedElements.ForEach(pair =>
                {
                    pair.Container.IsSelected = false;
                });

                _selectedElements.Clear();
                SetSelectedItems();

                _latestSelectedContainer = null;
                IsChangingSelection = false;
            }
        }

        internal void ResetSelectedElements()
        {
            _selectedElements.Clear();

            ForearchTreeViewItem(this, null, true,
                   (container, item) =>
                   {
                       if (container.IsSelected)
                           _selectedElements.Add(new ContainerItemPair(container, item));
                   });

            SetSelectedItems();
        }

        private bool RemoveFromSelectedElements(MultiSelectTreeViewItem treeViewItem)
        {
            var index = _selectedElements.FindIndex(p => p.Container == treeViewItem);

            if (index >= 0)
            {
                _selectedElements.RemoveAt(index);

                if (treeViewItem.IsSelected)
                    treeViewItem.IsSelected = false;

                return true;
            }

            return false;
        }

        private bool AddToSelectedElements(MultiSelectTreeViewItem treeViewItem, object item)
        {
            var index = _selectedElements.FindIndex(p => p.Container == treeViewItem);

            if (index < 0)
            {
                if (!treeViewItem.IsSelected)
                    treeViewItem.IsSelected = true;

                _selectedElements.Add(new ContainerItemPair(treeViewItem, item));
                return true;
            }

            return false;
        }

        private void ForearchTreeViewItem(ItemsControl itemsControl, object item, bool walkIntoSubtree, Action<MultiSelectTreeViewItem, object> doAction)
        {
            if (!(itemsControl is MultiSelectTreeView) && doAction != null)
                doAction(itemsControl as MultiSelectTreeViewItem, item);

            if (!itemsControl.HasItems || !(itemsControl is MultiSelectTreeView) && !walkIntoSubtree)
                return;

            for (int i = 0; i < itemsControl.Items.Count; i++)
            {
                var container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as MultiSelectTreeViewItem;
                if (container != null && doAction != null)
                    ForearchTreeViewItem(container, itemsControl.Items[i], walkIntoSubtree, doAction);
            }
        }

        internal void RemoveSelectedElement(MultiSelectTreeViewItem treeViewItem, object item, bool walkIntoSubtree)
        {
            bool flag = false;
            ForearchTreeViewItem(treeViewItem, item, walkIntoSubtree,
                (container, _) =>
                {
                    if (RemoveFromSelectedElements(container))
                        flag = true;
                });

            if (flag)
                SetSelectedItems();
        }

        internal void RemoveSelectedElementsWithInvalidContainer(bool isUpdateSelection)
        {
            var flag = false;
            for (int i = 0; i < _selectedElements.Count; i++)
            {
                var pair = _selectedElements[i];

                var itemsControl = ItemsControl.ItemsControlFromItemContainer(pair.Container);
                if (itemsControl == null)
                {
                    _selectedElements.RemoveAt(i);
                    i--;

                    flag = true;
                }
            }

            if (flag && isUpdateSelection)
                SetSelectedItems();
        }

        private void SelectRange(double topOffset, double bottomOffset, ItemsControl itemsControl)
        {
            if (itemsControl == null || !itemsControl.HasItems)
                return;

            for (int i = 0; i < itemsControl.Items.Count; i++)
            {
                var container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as MultiSelectTreeViewItem;
                if (container != null && container.IsEnabled && container.IsVisible)
                {
                    var header = container.GetHeaderElement();
                    if (header != null && header.IsVisible)
                    {
                        var curOffset = container.TranslatePoint(new Point(), this).Y;
                        if (DoubleUtil.LessThanOrClose(curOffset, bottomOffset) && DoubleUtil.GreaterThanOrClose(curOffset, topOffset))
                        {
                            if (!container.IsSelected)
                                container.IsSelected = true;

                            AddToSelectedElements(container, itemsControl.Items[i]);
                        }
                        else
                        {
                            if (container.IsSelected)
                                container.IsSelected = false;

                            RemoveFromSelectedElements(container);
                        }
                    }

                    if (!container.IsExpanded || !container.HasItems)
                        continue;

                    SelectRange(topOffset, bottomOffset, container);
                }
            }
        }

        private void SelectRange(MultiSelectTreeViewItem beginContainer, MultiSelectTreeViewItem endContainer)
        {
            if (Items == null || Items.Count == 0)
                return;

            var beginYOffset = beginContainer == null ? 0d : beginContainer.TranslatePoint(new Point(), this).Y;
            var endYOffset = endContainer == null ? 0d : endContainer.TranslatePoint(new Point(), this).Y;

            var topOffset = Math.Min(beginYOffset, endYOffset);
            var bottomOffset = Math.Max(beginYOffset, endYOffset);

            SelectRange(topOffset, bottomOffset, this);
        }

        internal void ChangeSelection(object data, MultiSelectTreeViewItem container, bool selected, SelectionMode selectionMode)
        {
            IsChangingSelection = true;
            bool flag = false;

            var mode = SelectionMode == SelectionMode.Multiple ? SelectionMode.Multiple : (SelectionMode)Math.Min((int)SelectionMode, (int)selectionMode);

            if (selected)
            {
                if (mode == SelectionMode.Single)
                {
                    _selectedElements.ForEach(p => p.Container.IsSelected = false);
                    _selectedElements.Clear();
                }

                if (mode == SelectionMode.Extended)
                {
                    SelectRange(_latestSelectedContainer, container);
                    flag = true;
                }
                else
                {
                    var pair = _selectedElements.FirstOrDefault(p => p.Container == container);
                    if (pair == null)
                    {
                        if (!container.IsSelected)
                            container.IsSelected = true;

                        _selectedElements.Add(new ContainerItemPair(container, data));
                        flag = true;
                    }
                    else if (pair.Item != data)
                    {
                        pair.Item = data;
                        flag = true;
                    }
                }
            }
            else
            {
                if (RemoveFromSelectedElements(container))
                    flag = true;
            }

            if (flag)
            {
                if (selected && mode != SelectionMode.Extended)
                    _latestSelectedContainer = (_selectedElements == null || _selectedElements.Count == 0) ? null : _selectedElements.Last().Container;

                if (!selected)
                    _latestSelectedContainer = container;

                SetSelectedItems();
            }

            IsChangingSelection = false;
        }

        private void UpdateSelectedValue(object selectedItem)
        {
            var binding = PrepareSelectedValuePathBinding(selectedItem);
            if (binding == null)
            {
                BindingOperations.ClearBinding(this, SelectedValueProperty);
                SelectedValue = null;
                return;
            }

            SetBinding(SelectedValueProperty, binding);
        }

        private bool IsXmlNode(object item)
        {
            if (item == null)
                return false;

            if (!item.GetType().FullName.StartsWith("System.Xml", StringComparison.Ordinal))
                return false;

            return item is XmlNode;
        }

        private Binding PrepareSelectedValuePathBinding(object item)
        {
            if (item == null)
                return null;

            var useXml = IsXmlNode(item);
            var binding = _selectedValueBinding;

            if (binding != null)
            {
                var usesXml = (binding.XPath != null);
                if ((!usesXml && useXml) || (usesXml && !useXml) || binding.Source != item)
                    binding = null;
            }

            if (binding == null)
            {
                binding = new Binding { Source = item };

                if (!useXml)
                    binding.Path = new PropertyPath(SelectedValuePath, new object[0]);
                else
                {
                    binding.XPath = SelectedValuePath;
                    binding.Path = new PropertyPath("/InnerText", new object[0]);
                }

                _selectedValueBinding = binding;
            }

            return binding;
        }

        #endregion

        #region Public Func

        public void ScrollToTop()
        {
            if (!HasItems)
                return;

            (ItemContainerGenerator.ContainerFromIndex(0) as FrameworkElement).BringIntoView();
        }

        public void ScrollToEnd()
        {
            if (!HasItems)
                return;

            MultiSelectTreeViewItem resultTreeViewItem = null;
            FindLastVisibleTreeViewItem(
                ItemContainerGenerator.ContainerFromIndex(Items.Count - 1) as MultiSelectTreeViewItem,
                ref resultTreeViewItem);

            if (resultTreeViewItem != null)
                resultTreeViewItem.BringIntoView();
        }

        public MultiSelectTreeViewItem ContainerFromSelectedItem()
        {
            if (_selectedElements == null || _selectedElements.Count == 0)
                return null;

            return _selectedElements[0].Container;
        }

        private void FindLastVisibleTreeViewItem(MultiSelectTreeViewItem parentTreeViewItem, ref MultiSelectTreeViewItem resultTreeViewItem)
        {
            if (!parentTreeViewItem.IsVisible)
                return;

            if (!parentTreeViewItem.HasItems)
            {
                resultTreeViewItem = parentTreeViewItem;
                return;
            }

            FindLastVisibleTreeViewItem(
                parentTreeViewItem.ItemContainerGenerator.ContainerFromIndex(parentTreeViewItem.Items.Count - 1) as MultiSelectTreeViewItem,
                ref resultTreeViewItem);
        }

        #endregion
    }
}