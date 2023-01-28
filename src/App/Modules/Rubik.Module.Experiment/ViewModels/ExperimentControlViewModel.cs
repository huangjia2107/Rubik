using System;
using System.Windows;

using Prism.Commands;
using Prism.Mvvm;

using Rubik.Module.Experiment.Models;
using Rubik.Module.Experiment.Views;
using Rubik.Theme.Extension.Controls;

namespace Rubik.Module.Experiment.ViewModels
{
    public class ExperimentControlViewModel : BindableBase
    {
        private LiveXaml _liveXaml = null;

        public DelegateCommand<RoutedEventArgs> LoadedCommand { get; private set; }

        public ExperimentControlViewModel()
        {
            InitCommand();
        }

        #region Init

        private void InitCommand()
        {
            LoadedCommand = new DelegateCommand<RoutedEventArgs>(Loaded);
        }

        #endregion

        #region Command

        private void Loaded(RoutedEventArgs e)
        {
            var experimentControl = e.OriginalSource as ExperimentControl;

            _liveXaml = experimentControl.LiveXamlName;

            if (string.IsNullOrEmpty(_liveXaml.Text))
                _liveXaml.Text = ConstStrings.LiveXamlTemplate;
        }

        #endregion

        #region Property

        public Func<string> ResetXamlFunc => () => ConstStrings.LiveXamlTemplate;

        #endregion

    }
}
