using YpdfDesktop.ViewModels.Base;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class MergeViewModel : PdfToolViewModel
    {
        #region Commands

        #endregion

        #region Reactive Properties

        #endregion

        // Constructor for Designer
        public MergeViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public MergeViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
        }

        #region Protected Methods

        protected override void Execute()
        {
            throw new System.NotImplementedException();
        }

        protected override void Reset()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
