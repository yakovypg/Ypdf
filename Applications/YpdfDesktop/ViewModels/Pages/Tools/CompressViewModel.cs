using YpdfDesktop.ViewModels.Base;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class CompressViewModel : PdfToolViewModel
    {
        #region Commands

        #endregion

        #region Reactive Properties

        #endregion

        // Constructor for Designer
        public CompressViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public CompressViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
        }
    }
}
