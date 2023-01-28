using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Prism.Mvvm;
using Rubik.Service.WebApi.Github;

namespace Rubik.Module.Github.ViewModels
{
    public class GithubReleaseViewModel : BindableBase
    {
        private GithubReleaseModel _model = null;
        private bool _isCurrentVersion = false;

        public GithubReleaseViewModel(GithubReleaseModel model, string version= "1.0")
        {
            _model = model;
            _isCurrentVersion = _model.tag_name.Equals($"v{version}", StringComparison.OrdinalIgnoreCase);
        }

        public string Name => _model.name;
        public DateTime Time => DateTime.Parse(_model.published_at).ToLocalTime();
        
        public bool IsPrerelease => _model.prerelease;

        public string Version => _model.tag_name.Replace("v", "", StringComparison.OrdinalIgnoreCase);
        public string VersionFlag => _isCurrentVersion ? "C":"R";
        public Brush VersionBackground => _isCurrentVersion ? Brushes.Green : Brushes.Orange;

        public FontStyle NotesFontStyle => string.IsNullOrWhiteSpace(_model.body) ? FontStyles.Oblique : FontStyles.Normal;
        public double NotesOpacity => string.IsNullOrWhiteSpace(_model.body) ? 0.5 : 1;
        public string Notes => string.IsNullOrWhiteSpace(_model.body) ? "No Description" : _model.body;

        public GithubReleaseAssetViewModel[] Assets => _model.assets.Select(a => new GithubReleaseAssetViewModel(a)).ToArray();
    }
}
