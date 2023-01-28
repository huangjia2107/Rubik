
namespace Rubik.Service.Models
{
    public enum DemoType
    {
        Button,
        Collection,
        Chart,
        Date,
        Layout,
        Media,
        Motion,
        Text,
        Slider,
        Style,
        Misc
    }

    public enum Location
    {
        GlobalConfigFile,
        ModulePath,
        DemoPath,
        ProcdumpPath,
        ProcdumpBatFile,
    }

    public enum MessageType
    {
        None,
        Error,
        Warning,
        Information,
        Question
    }

    public enum MessageButton
    {
        OK,
        OKCancel,
        YesNoCancel,
        YesNo
    }
}
