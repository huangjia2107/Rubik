using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using Rubik.Theme.Datas;
using Rubik.Theme.Panels;
using Rubik.Toolkit.Utils;

namespace Rubik.Theme.Primitives
{
    public sealed class AnchorPopup
    {
        private static readonly AnchorPopup _anchorPopup = new AnchorPopup();
        private readonly double _cornerRadius = 4;

        private Popup _popup;
        private Grid _grid;
        private AnchorBorder _anchorBorder;
        private ContentPresenter _contentPresenter;

        private AnchorPopup()
        {
            Construct();
        }

        public static void Show(UIElement placementTarget, object content, string background = "#DDDDDD", string foreground = "#404040", string borderBrush = "#404040", double borderThickness = 0)
        {
            _anchorPopup.InnerShow(placementTarget, content, background, foreground, borderBrush, borderThickness);
        }

        private void InnerShow(UIElement placementTarget, object content, string background, string foreground, string borderBrush, double borderThickness)
        {
            if (placementTarget == null || content == null)
                throw new ArgumentNullException();

            _contentPresenter.Content = content is string ? ConstructTextBlock((string)content) : content;

            TextBlock.SetForeground(_anchorBorder, ColorUtil.GetBrushFromString(foreground));

            _anchorBorder.Background = ColorUtil.GetBrushFromString(background);
            _anchorBorder.BorderBrush = ColorUtil.GetBrushFromString(borderBrush);
            _anchorBorder.BorderThickness = borderThickness;

            _popup.PlacementTarget = placementTarget;

            _popup.IsOpen = true;
        }

        private void Construct()
        {
            _contentPresenter = new ContentPresenter();
            _anchorBorder = ConstructAnchorBorder();
            _grid = new Grid();
            _popup = ConstructPopup();

            _anchorBorder.Child = _contentPresenter;
            _grid.Children.Add(_anchorBorder);

            _popup.Child = _grid;
        }

        private TextBlock ConstructTextBlock(string text)
        {
            return new TextBlock
            {
                Text = text,
                TextWrapping = TextWrapping.Wrap,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
        }

        private AnchorBorder ConstructAnchorBorder()
        {
            return new AnchorBorder
            {
                Background = Brushes.CadetBlue,
                Padding = new Thickness(10),
                VerticalAlignment = VerticalAlignment.Top,
                CornerRadius = new CornerRadius(_cornerRadius),
                BorderThickness = 0,
                AnchorDock = AnchorDock.BottomLeft,
                AnchorOffset = new Point(25, 15),
                DockOffset = 15,
                DockLength = 20
            };
        }

        private Popup ConstructPopup()
        {
            var popup = new Popup
            {
                Placement = PlacementMode.Top,
                StaysOpen = false,
                PopupAnimation = PopupAnimation.Fade,
                AllowsTransparency = true,
                MaxWidth = 300,
            };
            popup.Opened += Popup_OnOpened;

            return popup;
        }

        private void Popup_OnOpened(object sender, EventArgs e)
        {
            if (_popup == null || _grid == null || _anchorBorder == null)
                throw new ArgumentNullException("Popup_OnOpened");

            var placementTarget = _popup.PlacementTarget as FrameworkElement;

            if (placementTarget != null && _popup.Child != null)
            {
                var topLeftPoint = _popup.Child.TranslatePoint(new Point(), placementTarget);

                var topLeftToTargetPoint = new Point(Math.Round(topLeftPoint.X, 0), topLeftPoint.Y);
                var leftRightToTargetPoint = new Point(Math.Round(topLeftPoint.X, 0) + _grid.ActualWidth, topLeftToTargetPoint.Y);  //  testBtn.PointFromScreen(new Point(rect.Right, rect.Top));

                var isAbove = topLeftToTargetPoint.Y < 0;

                _anchorBorder.AnchorDock = isAbove ? AnchorDock.BottomLeft : AnchorDock.TopLeft;
                _anchorBorder.VerticalAlignment = isAbove ? VerticalAlignment.Top : VerticalAlignment.Bottom;

                _anchorBorder.DockOffset = GetDockOffset(_anchorBorder, placementTarget, topLeftToTargetPoint, leftRightToTargetPoint, isAbove);
                _anchorBorder.AnchorOffset = GetAnchorOffset(_anchorBorder, placementTarget, topLeftToTargetPoint, leftRightToTargetPoint, isAbove);

                _grid.Height = _anchorBorder.ActualRenderHeight;
            }

            /* 
            var fromVisual = (HwndSource)PresentationSource.FromVisual(_popup.Child);

            if (fromVisual != null && placementTarget != null)
            {
                Win32.RECT rect;

                if (Win32.GetWindowRect(fromVisual.Handle, out rect))
                {
                    var topLeftPoint = placementTarget.PointFromScreen(new Point(rect.Left, rect.Top));
                    var topLeftToTargetPoint = new Point(topLeftPoint.X + _anchorBorder.CornerRadius.TopLeft, topLeftPoint.Y);
                    var topRightToTargetPoint = new Point(topLeftPoint.X + _grid.ActualWidth - _anchorBorder.CornerRadius.TopRight, topLeftToTargetPoint.Y + _grid.ActualHeight);  //  testBtn.PointFromScreen(new Point(rect.Right, rect.Top));

                    bool isAbove = (topLeftToTargetPoint.Y < 0);

                    _anchorBorder.AnchorDock = isAbove ? AnchorDock.BottomLeft : AnchorDock.TopLeft;
                    _anchorBorder.VerticalAlignment = isAbove ? VerticalAlignment.Top : VerticalAlignment.Bottom;

                    _anchorBorder.AnchorOffset = GetAnchorOffset(_anchorBorder, placementTarget, topLeftToTargetPoint, topRightToTargetPoint, isAbove);
                    _anchorBorder.DockOffset = GetDockOffset(_anchorBorder, placementTarget, topLeftToTargetPoint, topRightToTargetPoint, isAbove);

                    _grid.Height = _anchorBorder.ActualRenderHeight;
                }
            }
            */
        }

        private double GetDockOffset(AnchorBorder anchorBorder, FrameworkElement placementTarget, Point topLeftToTargetPoint, Point leftRightToTargetPoint, bool isAbove)
        {
            if (anchorBorder == null || placementTarget == null)
                throw new ArgumentNullException(anchorBorder == null ? "anchorBorder" : "placementTarget");

            if (DoubleUtil.GreaterThanOrClose(topLeftToTargetPoint.X, 0))
            {
                return Math.Max(
                    _cornerRadius,
                    Math.Min(placementTarget.ActualWidth - topLeftToTargetPoint.X, anchorBorder.ActualWidth) / 2 - anchorBorder.DockLength / 2);
            }

            if (DoubleUtil.GreaterThanOrClose(anchorBorder.ActualWidth + topLeftToTargetPoint.X, 2 * _cornerRadius + anchorBorder.DockLength))
            {
                return Math.Max(
                    _cornerRadius,
                    (anchorBorder.ActualWidth + topLeftToTargetPoint.X) / 2 - anchorBorder.DockLength + Math.Abs(topLeftToTargetPoint.X));
            }

            return anchorBorder.ActualWidth - _cornerRadius - anchorBorder.DockLength;
        }

        private Point GetAnchorOffset(AnchorBorder anchorBorder, FrameworkElement placementTarget, Point topLeftToTargetPoint, Point leftRightToTargetPoint, bool isAbove)
        {
            if (anchorBorder == null || placementTarget == null)
                throw new ArgumentNullException(anchorBorder == null ? "anchorBorder" : "placementTarget");

            var anchorYOffset = isAbove ? Math.Abs(anchorBorder.AnchorOffset.Y) : -Math.Abs(anchorBorder.AnchorOffset.Y);

            if (DoubleUtil.GreaterThanOrClose(topLeftToTargetPoint.X, 0))
            {
                var validWidth = Math.Min(placementTarget.ActualWidth - topLeftToTargetPoint.X, anchorBorder.ActualWidth) / 2;
                var anchorXOffset = Math.Min(validWidth, anchorBorder.DockOffset + anchorBorder.DockLength / 2);

                return new Point(anchorXOffset, anchorYOffset);
            }

            if (DoubleUtil.GreaterThanOrClose(anchorBorder.ActualWidth + topLeftToTargetPoint.X, 2 * _cornerRadius + anchorBorder.DockLength))
            {
                return new Point(anchorBorder.DockOffset + anchorBorder.DockLength / 2, anchorYOffset);
            }

            return new Point(Math.Abs(topLeftToTargetPoint.X), anchorYOffset);
        }
    }
}
