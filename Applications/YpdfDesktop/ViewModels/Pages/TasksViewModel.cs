using System.Collections.ObjectModel;
using YpdfDesktop.Models.Informing;
using YpdfDesktop.ViewModels.Base;

namespace YpdfDesktop.ViewModels.Pages
{
    public class TasksViewModel : ViewModelBase
    {
        #region Observable Collections

        public ObservableCollection<TaskExecutionInfo> Tasks { get; }

        #endregion

        public TasksViewModel()
        {
            Tasks = new ObservableCollection<TaskExecutionInfo>();
        }
    }
}
