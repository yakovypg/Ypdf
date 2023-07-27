using DynamicData;
using ReactiveUI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Models.Informing;
using YpdfDesktop.ViewModels.Base;

namespace YpdfDesktop.ViewModels.Pages
{
    public class TasksViewModel : ViewModelBase
    {
        #region Commands

        public ReactiveCommand<IList, Unit> DeleteTasksCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteFinishedTasksCommand { get; }

        #endregion

        #region View Models

        public SettingsViewModel SettingsVM { get; }

        #endregion

        #region Reactive Properties

        private int _runningTasksCount = 0;
        public int RunningTasksCount
        {
            get => _runningTasksCount;
            set => this.RaiseAndSetIfChanged(ref _runningTasksCount, value);
        }

        private int _completedTasksCount = 0;
        public int CompletedTasksCount
        {
            get => _completedTasksCount;
            set => this.RaiseAndSetIfChanged(ref _completedTasksCount, value);
        }

        private int _faultedTasksCount = 0;
        public int FaultedTasksCount
        {
            get => _faultedTasksCount;
            set => this.RaiseAndSetIfChanged(ref _faultedTasksCount, value);
        }

        #endregion

        #region Observable Collections

        public ObservableCollection<ToolExecutionInfo> Tasks { get; }

        #endregion

        // Constructor for Designer
        public TasksViewModel() : this(new SettingsViewModel())
        {
        }

        public TasksViewModel(SettingsViewModel settingsVM)
        {
            SettingsVM = settingsVM;
            Tasks = new ObservableCollection<ToolExecutionInfo>();

            Tasks.CollectionChanged += TasksCollectionChanged;

            DeleteTasksCommand = ReactiveCommand.Create<IList>(DeleteTasks);
            DeleteFinishedTasksCommand = ReactiveCommand.Create(DeleteFinishedTasks);
        }

        #region Private Methods

        private void DeleteTasks(IList tasksList)
        {
            IEnumerable<ToolExecutionInfo> tasksToDelete = tasksList.Cast<ToolExecutionInfo>();
            Tasks.RemoveMany(tasksToDelete);
        }

        private void DeleteFinishedTasks()
        {
            IEnumerable<ToolExecutionInfo> tasksToDelete = Tasks.Where(t => t.Status != ToolExecutionStatus.Running);
            Tasks.RemoveMany(tasksToDelete);
        }

        private void TasksCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var (oldRunningTasks, oldCompletedTasks, oldFaultedTasks) = ClassifyTasks(e.OldItems);
            var (newRunningTasks, newCompletedTasks, newFaultedTasks) = ClassifyTasks(e.NewItems);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    RunningTasksCount = 0;
                    CompletedTasksCount = 0;
                    FaultedTasksCount = 0;
                    break;

                default:
                    RunningTasksCount += newRunningTasks - oldRunningTasks;
                    CompletedTasksCount += newCompletedTasks - oldCompletedTasks;
                    FaultedTasksCount += newFaultedTasks - oldFaultedTasks;
                    break;
            }
        }

        private static (int runningTasks, int completedTasks, int faultedTasks) ClassifyTasks(IList? tasks)
        {
            if (tasks is null)
                return (0, 0, 0);

            IEnumerable<ToolExecutionInfo> tasksEnumerable = tasks.Cast<ToolExecutionInfo>();

            int runningTasks = tasksEnumerable.Count(t => t.Status == ToolExecutionStatus.Running);
            int completedTasks = tasksEnumerable.Count(t => t.Status == ToolExecutionStatus.Completed);
            int faultedTasks = tasksEnumerable.Count(t => t.Status == ToolExecutionStatus.Faulted);

            return (runningTasks, completedTasks, faultedTasks);
        }

        #endregion
    }
}
