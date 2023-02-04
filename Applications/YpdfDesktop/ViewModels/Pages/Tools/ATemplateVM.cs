using YpdfDesktop.ViewModels.Base;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class ATemplateVM : PdfToolViewModel
    {
        #region Commands

        #endregion

        #region Reactive Properties

        #endregion

        // Constructor for Designer
        public ATemplateVM() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public ATemplateVM(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
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

        #region Private Methods

        #endregion
    }
}
