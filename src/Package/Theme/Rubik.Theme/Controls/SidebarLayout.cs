using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

using Rubik.Toolkit.Utils;
using Rubik.Theme.Animations;

namespace Rubik.Theme.Controls
{
    [TemplatePart(Name = SidebarColumnTemplateName, Type = typeof(ColumnDefinition))]
    public class SidebarLayout : ContentControl
    {
        private const string SidebarColumnTemplateName = "PART_SidebarColumn";

        private ColumnDefinition _sidebarColumn = null;

        private double _lastSidebarWidth = 0;
        private readonly Storyboard _timelineSb = new Storyboard();

        public SidebarLayout()
        {
            _lastSidebarWidth = SidebarMaxWidth;
        }

        static SidebarLayout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarLayout), new FrameworkPropertyMetadata(typeof(SidebarLayout)));
        }

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(SidebarLayout), new PropertyMetadata(true, OnIsExpandedChanged));
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var layout = (SidebarLayout)d;
            layout.Expand((bool)e.NewValue);
        }

        public static readonly DependencyProperty SidebarMaxWidthProperty = DependencyProperty.Register("SidebarMaxWidth", typeof(double), typeof(SidebarLayout),
            new PropertyMetadata(280d, null, CoerceSidebarMaxWidth));
        public double SidebarMaxWidth
        {
            get { return (double)GetValue(SidebarMaxWidthProperty); }
            set { SetValue(SidebarMaxWidthProperty, value); }
        }

        private static object CoerceSidebarMaxWidth(DependencyObject d, object value)
        {
            var minwidth = ((SidebarLayout)d).SidebarMinWidth;
            var val = (double)value;

            return DoubleUtil.LessThan(val, minwidth) ? minwidth : val;
        }

        public static readonly DependencyProperty SidebarMinWidthProperty = DependencyProperty.Register("SidebarMinWidth", typeof(double), typeof(SidebarLayout),
            new PropertyMetadata(50d, OnSidebarMinWidthChanged));
        public double SidebarMinWidth
        {
            get { return (double)GetValue(SidebarMinWidthProperty); }
            set { SetValue(SidebarMinWidthProperty, value); }
        }

        private static void OnSidebarMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var layout = (SidebarLayout)d;

            layout.CoerceValue(SidebarMaxWidthProperty, layout.SidebarMaxWidth);
        }

        public static readonly DependencyProperty SidebarContentProperty = DependencyProperty.Register("SidebarContent", typeof(object), typeof(SidebarLayout),
            new FrameworkPropertyMetadata(null, OnContentChanged));
        public object SidebarContent
        {
            get { return GetValue(SidebarContentProperty); }
            set { SetValue(SidebarContentProperty, value); }
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (SidebarLayout)d;
            ctrl.OnContentChanged(e.OldValue, e.NewValue);
        }

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _sidebarColumn = GetTemplateChild(SidebarColumnTemplateName) as ColumnDefinition;
        }

        #endregion

        #region Func

        private void Expand(bool isExpanded)
        {
            GridLengthAnimation animation;

            if (_timelineSb.Children.Count > 0)
                animation = _timelineSb.Children[0] as GridLengthAnimation;
            else
            {
                animation = new GridLengthAnimation();
                animation.Completed += Animation_Completed;

                _timelineSb.Children.Add(animation);
            }

            if (isExpanded)
            {
                if (DoubleUtil.AreClose(_sidebarColumn.ActualWidth, SidebarMinWidth))
                {
                    _lastSidebarWidth = DoubleUtil.AreClose(_lastSidebarWidth, SidebarMinWidth) ? SidebarMaxWidth : _lastSidebarWidth;

                    animation.From = new GridLength(SidebarMinWidth);
                    animation.To = new GridLength(_lastSidebarWidth);
                }
                else
                {
                    _lastSidebarWidth = _sidebarColumn.ActualWidth;

                    IsExpanded = false;
                    animation.From = new GridLength(_lastSidebarWidth);
                    animation.To = new GridLength(SidebarMinWidth);
                }
            }
            else
            {
                if (DoubleUtil.AreClose(_sidebarColumn.ActualWidth, SidebarMinWidth))
                {
                    _lastSidebarWidth = DoubleUtil.AreClose(_lastSidebarWidth, SidebarMinWidth) ? SidebarMaxWidth : _lastSidebarWidth;

                    IsExpanded = true;
                    animation.From = new GridLength(SidebarMinWidth);
                    animation.To = new GridLength(_lastSidebarWidth);
                }
                else
                {
                    _lastSidebarWidth = _sidebarColumn.ActualWidth;

                    animation.From = new GridLength(_lastSidebarWidth);
                    animation.To = new GridLength(SidebarMinWidth);
                }
            }

            animation.FillBehavior = FillBehavior.Stop;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(80));
            animation.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut };

            Storyboard.SetTarget(animation, _sidebarColumn);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(ColumnDefinition.Width)"));

            _timelineSb.Begin();
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            _sidebarColumn.BeginAnimation(ColumnDefinition.WidthProperty, null);
            _sidebarColumn.Width = IsExpanded ? new GridLength(_lastSidebarWidth) : new GridLength(SidebarMinWidth);
        }

        private void CoerceValue(DependencyProperty dp, object localValue)
        {
            //Update local value firstly
            SetCurrentValue(dp, localValue);
            CoerceValue(dp);
        }

        #endregion
    }
}
