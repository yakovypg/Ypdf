namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class CompressViewModel : ViewModelBase
    {
        #region Commands

        #endregion

        #region View Models

        public SettingsViewModel SettingsVM { get; }

        #endregion

        #region Reactive Properties

        #endregion

        // Constructor for Designer
        public CompressViewModel() : this(new SettingsViewModel())
        {
        }

        public CompressViewModel(SettingsViewModel settingsVM)
        {
            SettingsVM = settingsVM;
        }
    }
}
