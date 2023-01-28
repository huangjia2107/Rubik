
namespace Rubik.Theme.Datas
{   
    public enum AnchorDock
    {
        None = 0,

        LeftTop,
        LeftCenter,
        LeftBottom,

        TopLeft,
        TopCenter,
        TopRight,

        RightTop,
        RightCenter,
        RightBottom,

        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public enum TextCase
    {
        Normal,
        Upper,
        Lower
    }

    public enum MarkDock
    {
        Up,
        Down
    }

    public enum RulerUnit
    {
        Pixel,
        Millimeter,
        Centimeter,
        Inch,
        Foot
    }

    public enum OverlapArea
    {
        Out = 0,
        Inner = 1,
        Up = 2,
        Down = 4,
        Left = 8,
        Right = 16
    }
}
