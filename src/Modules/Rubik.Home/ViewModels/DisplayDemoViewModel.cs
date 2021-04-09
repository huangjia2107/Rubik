using System.Collections.Generic;
using Prism.Mvvm;
using Rubik.Service.Models;

namespace Rubik.Home.ViewModels
{
    public class DisplayDemoViewModel : BindableBase
    {
        public DemoType Type { get; set; }
        public List<DemoViewModel> DemoList { get; set; }

        public int Count => DemoList.Count;
    }
}
