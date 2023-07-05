using ReactiveUI;
using System.Reactive;
using YpdfDesktop.ViewModels.Base;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class A0TemplateVM : PdfToolViewModel
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }

        #endregion

        #region Reactive Properties

        #endregion

        // Constructor for Designer
        public A0TemplateVM() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public A0TemplateVM(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
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
