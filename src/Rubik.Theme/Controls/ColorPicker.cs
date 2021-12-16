﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

using Rubik.Toolkit.Utils;

namespace Rubik.Theme.Controls
{
    [TemplatePart(Name = SelectPathTemplateName, Type = typeof(Path))]
    [TemplatePart(Name = AlphaSliderTemplateName, Type = typeof(Slider))]
    [TemplatePart(Name = HueSliderTemplateName, Type = typeof(Slider))]
    [TemplatePart(Name = ColorCanvasTemplateName, Type = typeof(Canvas))]
    public class ColorPicker : Control
    {
        private static readonly Type _typeofSelf = typeof(ColorPicker);

        private const string AlphaSliderTemplateName = "PART_AlphaSlider";
        private const string HueSliderTemplateName = "PART_HueSlider";
        private const string SelectPathTemplateName = "PART_SelectPath";
        private const string ColorCanvasTemplateName = "PART_ColorCanvas";

        private Slider _alphaSlider = null;
        private Slider _hueSlider = null;
        private Path _selectPath = null;
        private Canvas _colorCanvas = null;

        private Point _mousePosToSelectPath;
        private bool _isInnerUpdateSelectedColor = false;

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        #region Properties

        private static readonly DependencyPropertyKey HueColorPropertyKey =
           DependencyProperty.RegisterReadOnly("HueColor", typeof(Color), _typeofSelf, new PropertyMetadata(Colors.Red));
        public static readonly DependencyProperty HueColorProperty = HueColorPropertyKey.DependencyProperty;
        public Color HueColor
        {
            get { return (Color)GetValue(HueColorProperty); }
        }

        private static readonly DependencyPropertyKey HPropertyKey =
           DependencyProperty.RegisterReadOnly("H", typeof(double), _typeofSelf, new PropertyMetadata(1d));
        public static readonly DependencyProperty HProperty = HPropertyKey.DependencyProperty;
        public double H
        {
            get { return (double)GetValue(HProperty); }
        }

        private static readonly DependencyPropertyKey SPropertyKey =
           DependencyProperty.RegisterReadOnly("S", typeof(double), _typeofSelf, new PropertyMetadata(0d));
        public static readonly DependencyProperty SProperty = SPropertyKey.DependencyProperty;
        public double S
        {
            get { return (double)GetValue(SProperty); }
        }

        private static readonly DependencyPropertyKey BPropertyKey =
           DependencyProperty.RegisterReadOnly("B", typeof(double), _typeofSelf, new PropertyMetadata(0d));
        public static readonly DependencyProperty BProperty = BPropertyKey.DependencyProperty;
        public double B
        {
            get { return (double)GetValue(BProperty); }
        }

        private static readonly DependencyPropertyKey LastColorPropertyKey =
           DependencyProperty.RegisterReadOnly("LastColor", typeof(Color), _typeofSelf, new PropertyMetadata(Colors.Black));
        public static readonly DependencyProperty LastColorProperty = LastColorPropertyKey.DependencyProperty;
        public Color LastColor
        {
            get { return (Color)GetValue(LastColorProperty); }
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), _typeofSelf, new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as ColorPicker;
            if (ctrl._isInnerUpdateSelectedColor)
            {
                ctrl._isInnerUpdateSelectedColor = false;
                return;
            }

            var color = (Color)e.NewValue;
            var hsb = ColorUtil.HsbFromColor(color);

            ctrl.SetValue(HPropertyKey, hsb.H);
            ctrl.SetValue(SPropertyKey, hsb.S);
            ctrl.SetValue(BPropertyKey, hsb.B);

            ctrl.SetValue(LastColorPropertyKey, color);

            ctrl.UpdateHueColor();
            ctrl.UpdateSelectedPathPosition();
            ctrl.UpdateHueSliderPosition();
            ctrl.UpdateAlphaSliderPosition();
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            if (_alphaSlider != null)
            {
                _alphaSlider.ValueChanged -= OnAlphaSliderValueChanged;
                _alphaSlider.MouseMove -= OnAlphaSliderMouseMove;
                _alphaSlider.RemoveHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnHueSliderMouseLeftButtonUp));
            }

            if (_hueSlider != null)
            {
                _hueSlider.ValueChanged -= OnHueSliderValueChanged;
                _hueSlider.MouseMove -= OnHueSliderMouseMove;
                _hueSlider.RemoveHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnAlphaSliderMouseLeftButtonUp));
            }

            _colorCanvas?.RemoveHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnColorCanvasMouseLeftButtonDown));

            base.OnApplyTemplate();

            _alphaSlider = base.GetTemplateChild(AlphaSliderTemplateName) as Slider;
            _hueSlider = base.GetTemplateChild(HueSliderTemplateName) as Slider;
            _selectPath = base.GetTemplateChild(SelectPathTemplateName) as Path;
            _colorCanvas = base.GetTemplateChild(ColorCanvasTemplateName) as Canvas;

            if (_alphaSlider != null)
            {
                _alphaSlider.ValueChanged += OnAlphaSliderValueChanged;
                _alphaSlider.MouseMove += OnAlphaSliderMouseMove;
                _alphaSlider.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnAlphaSliderMouseLeftButtonUp), true);
            }

            if (_hueSlider != null)
            {
                _hueSlider.ValueChanged += OnHueSliderValueChanged;
                _hueSlider.MouseMove += OnHueSliderMouseMove;
                _hueSlider.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnHueSliderMouseLeftButtonUp), true);
            }

            _colorCanvas?.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnColorCanvasMouseLeftButtonDown), false);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);

            UpdateSelectedPathPosition();
            UpdateHueSliderPosition();
            UpdateAlphaSliderPosition();

            return size;
        }

        #endregion

        #region Event

        private void OnAlphaSliderMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !(e.OriginalSource is Thumb))
            {
                _alphaSlider.CaptureMouse();
                _alphaSlider.Value = _alphaSlider.Maximum - e.GetPosition(_alphaSlider).Y / _alphaSlider.ActualHeight * (_alphaSlider.Maximum - _alphaSlider.Minimum);
            }

            e.Handled = true;
        }

        private void OnAlphaSliderMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetValue(LastColorPropertyKey, SelectedColor);

            _alphaSlider.ReleaseMouseCapture();
            e.Handled = true;
        }

        private void OnAlphaSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (DoubleUtil.AreClose(SelectedColor.A, e.NewValue))
                return;

            UpdateSelectedColor((byte)e.NewValue);
        }

        private void OnHueSliderMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !(e.OriginalSource is Thumb))
            {
                _hueSlider.CaptureMouse();
                _hueSlider.Value = 1 - e.GetPosition(_hueSlider).Y / _hueSlider.ActualHeight * (_hueSlider.Maximum - _hueSlider.Minimum);
            }

            e.Handled = true;
        }

        private void OnHueSliderMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetValue(LastColorPropertyKey, SelectedColor);

            _hueSlider.ReleaseMouseCapture();
            e.Handled = true;
        }

        private void OnHueSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (DoubleUtil.AreClose(H, e.NewValue))
                return;

            SetValue(HPropertyKey, e.NewValue);

            UpdateHueColor();
            UpdateSelectedColor(SelectedColor.A);
        }

        private void OnColorCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(_colorCanvas);

            Canvas.SetLeft(_selectPath, pos.X - _selectPath.ActualWidth / 2);
            Canvas.SetTop(_selectPath, pos.Y - _selectPath.ActualHeight / 2);

            UpdateSB();

            _selectPath.CaptureMouse();
            _selectPath.MouseMove += OnSelectPathMouseMove;
            _selectPath.LostMouseCapture += OnSelectPathLostMouseCapture;
            _selectPath.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnSelectPathMouseLeftButtonUp), false);
        }

        private void OnSelectPathMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas)
                _mousePosToSelectPath = new Point(_selectPath.ActualWidth / 2, _selectPath.ActualHeight / 2);
            else
                _mousePosToSelectPath = e.GetPosition(_selectPath);

            _selectPath.CaptureMouse();
            _selectPath.MouseMove += OnSelectPathMouseMove;
            _selectPath.LostMouseCapture += OnSelectPathLostMouseCapture;
            _selectPath.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnSelectPathMouseLeftButtonUp), false);
        }

        private void OnSelectPathMouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(_colorCanvas);

            Canvas.SetLeft(_selectPath, Math.Max(-(_selectPath.ActualWidth / 2), Math.Min(pos.X - _selectPath.ActualWidth / 2, _colorCanvas.ActualWidth - _selectPath.ActualWidth / 2)));
            Canvas.SetTop(_selectPath, Math.Max(-(_selectPath.ActualHeight / 2), Math.Min(pos.Y - _selectPath.ActualHeight / 2, _colorCanvas.ActualHeight - _selectPath.ActualHeight / 2)));

            UpdateSB();
        }

        private void OnSelectPathLostMouseCapture(object sender, MouseEventArgs e)
        {
            SetValue(LastColorPropertyKey, SelectedColor);

            _selectPath.MouseMove -= OnSelectPathMouseMove;
            _selectPath.LostMouseCapture -= OnSelectPathLostMouseCapture;
            _selectPath.RemoveHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnSelectPathMouseLeftButtonUp));
        }

        private void OnSelectPathMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _selectPath.ReleaseMouseCapture();
        }

        #endregion

        #region Func

        private void UpdateAlphaSliderPosition()
        {
            if (_alphaSlider == null)
                return;

            _alphaSlider.Value = SelectedColor.A;
        }

        private void UpdateHueSliderPosition()
        {
            if (_hueSlider == null)
                return;

            _hueSlider.Value = H;
        }

        private void UpdateSelectedPathPosition()
        {
            if (_selectPath == null || _colorCanvas == null)
                return;

            Canvas.SetLeft(_selectPath, S * _colorCanvas.ActualWidth - _selectPath.ActualWidth / 2);
            Canvas.SetTop(_selectPath, (1 - B) * _colorCanvas.ActualHeight - _selectPath.ActualHeight / 2);
        }

        private void UpdateSB()
        {
            SetValue(SPropertyKey, (Canvas.GetLeft(_selectPath) + _selectPath.ActualWidth / 2) / _colorCanvas.ActualWidth);
            SetValue(BPropertyKey, 1 - (Canvas.GetTop(_selectPath) + _selectPath.ActualHeight / 2) / _colorCanvas.ActualHeight);

            UpdateSelectedColor(SelectedColor.A);
        }

        private void UpdateHueColor()
        {
            SetValue(HueColorPropertyKey, ColorUtil.ColorFromHsb(H, 1, 1));
        }

        private void UpdateSelectedColor(byte a)
        {
            var currentColor = ColorUtil.ColorFromAhsb(a / 255d, H, S, B);
            if (SelectedColor != currentColor)
            {
                _isInnerUpdateSelectedColor = true;
                SelectedColor = currentColor;
            }
        }

        #endregion
    }
}

