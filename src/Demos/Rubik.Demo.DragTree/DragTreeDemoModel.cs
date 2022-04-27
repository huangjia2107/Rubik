﻿using Rubik.Service;
using Rubik.Service.Models;

namespace Rubik.Demo.DragTree
{
    public class DragTreeDemoModel : IDemoModel
    {
        public DemoType Type { get; set; } = DemoType.Collection;

        public string Name { get; set; } = "Drag On TreeView";

        public string IconData { get; set; } = "M320 224h-66v-56c0-4.4-3.6-8-8-8h-52c-4.4 0-8 3.6-8 8v56h-66c-4.4 0-8 3.6-8 8v560c0 4.4 3.6 8 8 8h66v56c0 4.4 3.6 8 8 8h52c4.4 0 8-3.6 8-8v-56h66c4.4 0 8-3.6 8-8V232c0-4.4-3.6-8-8-8z m-60 508h-80V292h80v440zM904 296h-66v-96c0-4.4-3.6-8-8-8h-52c-4.4 0-8 3.6-8 8v96h-66c-4.4 0-8 3.6-8 8v416c0 4.4 3.6 8 8 8h66v96c0 4.4 3.6 8 8 8h52c4.4 0 8-3.6 8-8v-96h66c4.4 0 8-3.6 8-8V304c0-4.4-3.6-8-8-8z m-60 364h-80V364h80v296zM612 404h-66V232c0-4.4-3.6-8-8-8h-52c-4.4 0-8 3.6-8 8v172h-66c-4.4 0-8 3.6-8 8v200c0 4.4 3.6 8 8 8h66v172c0 4.4 3.6 8 8 8h52c4.4 0 8-3.6 8-8V620h66c4.4 0 8-3.6 8-8V412c0-4.4-3.6-8-8-8z m-60 145c0 1.6-1.3 3-3 3h-74c-1.6 0-3-1.3-3-3v-74c0-1.6 1.3-3 3-3h74c1.6 0 3 1.3 3 3v74z";

        public double IconWidth { get; set; } = 16;

        public double IconHeight { get; set; } = 16;

        public object Page { get; set; } = new MainControl();
    }
}