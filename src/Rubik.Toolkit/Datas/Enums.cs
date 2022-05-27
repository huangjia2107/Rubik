
namespace Rubik.Toolkit.Datas
{   
    public enum RulerUnit
    {
        Pixel,
        Millimeter,
        Centimeter,
        Inch,
        Foot
    }

    public enum PageOrientation
    {
        Landscape,
        Portrait
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
