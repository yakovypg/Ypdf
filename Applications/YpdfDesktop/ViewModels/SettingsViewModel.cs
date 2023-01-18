using ReactiveUI;

namespace YpdfDesktop.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _pythonAlias = string.Empty;
        public string PythonAlias
        {
            get => _pythonAlias;
            set => this.RaiseAndSetIfChanged(ref _pythonAlias, value);
        }
    }
}
