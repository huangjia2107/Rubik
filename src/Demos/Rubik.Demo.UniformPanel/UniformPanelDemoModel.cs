using Rubik.Service;
using Rubik.Service.Models;

namespace Rubik.Demo.UniformPanel
{
    public class UniformPanelDemoModel : IDemoModel
    {
        public DemoType Type => DemoType.Layout;

        public string Name => "UniformPanel";

        public string IconData => "M288,736 L288,960 64,960 64,736 288,736 z M624,736 L624,960 400,960 400,736 624,736 z M288,400 L288,624 64,624 64,400 288,400 z M624,400 L624,624 400,624 400,400 624,400 z M960,400 L960,624 736,624 736,400 960,400 z M288,64 L288,288 64,288 64,64 288,64 z M624,64 L624,288 400,288 400,64 624,64 z M960,64 L960,288 736,288 736,64 960,64 z";

        public double IconWidth => 16;

        public double IconHeight => 16;

        public object Page => new MainControl();
    }
}