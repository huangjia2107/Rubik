﻿using Rubik.Service;
using Rubik.Service.Models;

using Rubik.Demo.DragList.Views;

namespace Rubik.Demo.DragList
{
    public class DragListDemoModel : IDemoModel
    {
        public DemoType Type { get; set; } = DemoType.Collection;

        public string Name { get; set; } = "Drag On ListBox";

        public string IconData { get; set; } = "M597.333333 256h85.333334v85.333333h213.333333a42.666667 42.666667 0 0 1 42.666667 42.666667v320L682.666667 554.666667l1.536 343.978666 94.848-91.733333L855.082667 938.666667H384a42.666667 42.666667 0 0 1-42.666667-42.666667v-213.333333H256v-85.333334h85.333333V384a42.666667 42.666667 0 0 1 42.666667-42.666667h213.333333V256z m341.333334 483.754667V896a42.666667 42.666667 0 0 1-2.048 13.098667l-83.626667-144.810667L938.666667 739.754667zM170.666667 597.333333v85.333334H85.333333v-85.333334h85.333334z m0-170.666666v85.333333H85.333333v-85.333333h85.333334z m0-170.666667v85.333333H85.333333V256h85.333334z m0-170.666667v85.333334H85.333333V85.333333h85.333334z m170.666666 0v85.333334H256V85.333333h85.333333z m170.666667 0v85.333334h-85.333333V85.333333h85.333333z m170.666667 0v85.333334h-85.333334V85.333333h85.333334z";

        public double IconWidth { get; set; } = 16;

        public double IconHeight { get; set; } = 16;

        public object Page { get; set; } = new MainControl();
    }
}