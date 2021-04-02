using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Prism.Mvvm;
using Rubik.Demo.Models;

namespace Rubik.Demo.ViewModels
{
    public class HomeControlViewModel : BindableBase
    {
        private AppData _appData = null;

        public HomeControlViewModel(AppData appData)
        {
            _appData = appData;
        }

        public List<DisplayDemoViewModel> AllDemos
        {
            get
            {
                if (_appData.InternalData.DemoModels == null || _appData.InternalData.DemoModels.Length == 0)
                    return null;

                return _appData.InternalData.DemoModels.GroupBy(d => d.Type)
                                                       .Select(g => new DisplayDemoViewModel 
                                                       { 
                                                           Type = g.Key, 
                                                           DemoList = g.Select(d => new DemoViewModel(d)).ToList() 
                                                       }).ToList();
            }
        }
    }
}
