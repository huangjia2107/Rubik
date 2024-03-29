﻿using Prism.Mvvm;

using Rubik.Module.Home.Models;
using Rubik.Service;
using Rubik.Service.Models;

namespace Rubik.Module.Home.ViewModels
{
    public class DemoViewModel : BindableBase
    {
        private IDemoModel _model = null;

        public DemoViewModel(IDemoModel model)
        {
            _model = model;
        }

        public DemoType Type => _model.Type;

        public string Name => _model.Name;

        public string IconData => _model.IconData;

        public double IconWidth => _model.IconWidth;

        public double IconHeight => _model.IconHeight;

        public object Page => _model.Page;

        public DemoExample[] Examples => (_model as ConfigDemoModel)?.Examples;
    }
}
