using Rubik.Service;
using Rubik.Service.Models;

namespace Rubik.Demo.FlowPanel
{
    public class FlowPanelDemoModel : IDemoModel
    {
        public DemoType Type { get; set; } = DemoType.Layout;

        public string Name { get; set; } = "FlowPanel";

        public string IconData { get; set; } = "M64 112h416v256H64v-256zM64 432h416v256H64v-256zM64 752h412.8v160H64v-160zM547.2 112H960v160H547.2v-160zM544 336h416v320H544v-320zM544 720h416v192H544v-192z";

        public double IconWidth { get; set; } = 16;

        public double IconHeight { get; set; } = 16;

        public object Page { get; set; } = new MainControl();
    }
}