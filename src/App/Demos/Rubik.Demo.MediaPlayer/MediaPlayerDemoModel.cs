using Rubik.Service;
using Rubik.Service.Models;

using Rubik.Demo.MediaPlayer.Views;

namespace Rubik.Demo.MediaPlayer
{
    public class MediaPlayerDemoModel : IDemoModel
    {
        public DemoType Type { get; set; } = DemoType.Media;

        public string Name { get; set; } = "Media Palyer";

        public string IconData { get; set; } = "M997.26976 209.877333a64.810667 64.810667 0 0 0-55.466667-53.205333 3234.133333 3234.133333 0 0 0-859.392 0 64.768 64.768 0 0 0-55.466666 53.162667 1707.562667 1707.562667 0 0 0 0 604.288 64.810667 64.810667 0 0 0 55.466666 53.205333A3233.408 3233.408 0 0 0 512.192427 896a3233.408 3233.408 0 0 0 429.696-28.672 64.768 64.768 0 0 0 55.466666-53.205333 1708.330667 1708.330667 0 0 0-0.085333-604.245334zM341.52576 640.853333V383.189333a44.373333 44.373333 0 0 1 65.450667-36.821333l252.586666 128.810667a40.832 40.832 0 0 1 0 73.642666l-252.586666 128.810667A44.373333 44.373333 0 0 1 341.52576 640.853333z";

        public double IconWidth { get; set; } = 16;

        public double IconHeight { get; set; } = 14;

        public object Page { get; set; } = new MainControl();
    }
}
