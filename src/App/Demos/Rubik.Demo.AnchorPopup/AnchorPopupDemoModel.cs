using Rubik.Service;
using Rubik.Service.Models;

namespace Rubik.Demo.AnchorPopup
{
    public class AnchorPopupDemoModel : IDemoModel
    {
        public DemoType Type { get; set; } = DemoType.Text;

        public string Name { get; set; } = "AnchorPopup";

        public string IconData { get; set; } = "M20,2H4A2,2 0 0,0 2,4V22L6,18H20A2,2 0 0,0 22,16V4A2,2 0 0,0 20,2M6,9H18V11H6M14,14H6V12H14M18,8H6V6H18";

        public double IconWidth { get; set; } = 16;

        public double IconHeight { get; set; } = 16;

        public object Page { get; set; } = new MainControl();
    }
}