using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;

using Rubik.Toolkit.Utils;

namespace Rubik.Toolkit.Adorners
{
    public class MouseTipAdorner : Adorner
    {
        private string _tip = null;

        private Brush _fgBrush = null;
        private Brush _bgBrush = null;

        public MouseTipAdorner(UIElement adornedElement, string tip)
            : base(adornedElement)
        {
            IsHitTestVisible = false;
            _tip = tip;

            _bgBrush = ColorUtil.GetBrushFromString("#BB007ACC");
            _bgBrush.Freeze();

            _fgBrush = Brushes.White;
            _fgBrush.Freeze();
        }

        public void Update()
        {
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement != null)
            {
                var screenPos = new Win32.POINT();
                if (Win32.GetCursorPos(ref screenPos))
                {
                    var pos = AdornedElement.PointFromScreen(new Point(screenPos.X + 10, screenPos.Y + 10));

                    var ft = DrawUtil.DrawText(_tip, _fgBrush);
                    var rect = new Rect(pos.X, pos.Y, ft.Width + 10, ft.Height + 6);

                    drawingContext.DrawRectangle(_bgBrush, null, rect);
                    drawingContext.DrawText(ft, new Point(pos.X + 5, pos.Y + 3));
                }
            }
        }
    }
}
