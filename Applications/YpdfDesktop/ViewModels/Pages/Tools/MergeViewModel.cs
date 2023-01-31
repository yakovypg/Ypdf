namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class MergeViewModel : ViewModelBase
    {
        #region Commands

        #endregion

        #region View Models

        public SettingsViewModel SettingsVM { get; }

        #endregion

        #region Reactive Properties

        #endregion

        // Constructor for Designer
        public MergeViewModel() : this(new SettingsViewModel())
        {
        }

        public MergeViewModel(SettingsViewModel settingsVM)
        {
            SettingsVM = settingsVM;
        }
    }
}
