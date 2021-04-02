using System.Collections.Generic;
using Prism.Mvvm;
using Rubik.Service.Models;

namespace Rubik.Demo.ViewModels
{
    public class DisplayDemoViewModel : BindableBase
    {
        public DemoType Type { get; set; }
        public List<DemoViewModel> DemoList { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetProperty(ref _isChecked, value); }
        }
    }
}
