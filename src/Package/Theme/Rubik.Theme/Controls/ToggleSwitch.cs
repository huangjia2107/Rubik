using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rubik.Theme.Controls
{
    [TemplatePart(Name = PART_ThumbIndicator, Type = typeof(Ellipse))]
    [TemplatePart(Name = PART_ThumbTranslate, Type = typeof(TranslateTransform))]
    public class ToggleSwitch : ButtonBase
    {
        private static readonly Type _typeofSelf = typeof(ToggleSwitch);

        private const string PART_ThumbIndicator = "PART_ThumbIndicator";
        private const string PART_ThumbTranslate = "PART_ThumbTranslate";

        private Ellipse _thumbIndicator = null;
        private TranslateTransform _thumbTranslate = null;

        private DoubleAnimation _animation = null;

        static ToggleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(_typeofSelf, new FrameworkPropertyMetadata(_typeofSelf));
        }

        public static readonly RoutedEvent SwitchChangedEvent = EventManager.RegisterRoutedEvent("SwitchChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<bool>), _typeofSelf);
        public event RoutedPropertyChangedEventHandler<bool> SwitchChanged
        {
            add { AddHandler(SwitchChangedEvent, value); }
            remove { RemoveHandler(SwitchChangedEvent, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), _typeofSelf,
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsCheckedChanged));
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ToggleSwitch)d;
            ctrl.OnIsCheckedChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        #region Virtual

        protected virtual void OnIsCheckedChanged(bool oldValue, bool newValue)
        {
            OnToggle(false);

            if (oldValue != newValue)
            {
                RaiseEvent(new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue, SwitchChangedEvent));
            }
        }

        #endregion

        #region Override

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);

            UpdateTranslate();

            return size;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _thumbIndicator = GetTemplateChild(PART_ThumbIndicator) as Ellipse;
            _thumbTranslate = GetTemplateChild(PART_ThumbTranslate) as TranslateTransform;

            UpdateTranslate();
        }

        protected override void OnClick()
        {
            OnToggle(true);
            base.OnClick();
        }

        #endregion

        #region Func

        private void UpdateTranslate()
        {
            if (_animation != null && _thumbTranslate != null)
                _thumbTranslate.BeginAnimation(TranslateTransform.XProperty, null);

            if (_thumbTranslate != null && _thumbIndicator != null)
                _thumbTranslate.X = GetThumbIndicatorDestination();
        }

        private double GetThumbIndicatorDestination()
        {
            return IsChecked ? ActualWidth - this.BorderThickness.Left - this.BorderThickness.Right - (_thumbIndicator.Margin.Left + _thumbIndicator.Margin.Right + _thumbIndicator.ActualWidth) : 0;
        }

        private void OnToggle(bool fromClick)
        {
            if (fromClick)
                IsChecked = !IsChecked;

            if (_thumbTranslate != null && _thumbIndicator != null)
            {
                if (_animation == null)
                {
                    _animation = new DoubleAnimation();
                    _animation.Duration = TimeSpan.FromMilliseconds(100);
                }

                _animation.To = GetThumbIndicatorDestination();
                _thumbTranslate.BeginAnimation(TranslateTransform.XProperty, _animation);
            }
        }

        #endregion
    }
}