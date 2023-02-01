using Avalonia.Controls;
using Avalonia.Threading;
using ExecutionLib.Configuration;
using ExecutionLib.Execution;
using ExecutionLib.Informing.Logging;
using MessageBox.Avalonia.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Infrastructure.Logging;
using YpdfDesktop.Infrastructure.Search;
using YpdfDesktop.Infrastructure.Services;
using YpdfDesktop.Models;
using YpdfDesktop.Models.Informing;
using YpdfDesktop.ViewModels.Pages;

namespace YpdfDesktop.ViewModels.Base
{
    public abstract class PdfToolViewModel : ViewModelBase
    {
        #region View Models

        public SettingsViewModel SettingsVM { get; }
        public TasksViewModel TasksVM { get; }

        #endregion

        public PdfToolViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM)
        {
            SettingsVM = settingsVM;
            TasksVM = tasksVM;
        }

        #region Abstract Methods

        protected abstract void Execute();
        protected abstract void Reset();

        #endregion

        #region Protected Methods

        protected async Task<bool> VerifyOutputPath(string? path)
        {
            if (!File.Exists(path))
                return true;

            if (WindowFinder.FindMainWindow() is not Window mainWindow)
                return false;

            string? fileExistsMessage = SettingsVM.Locale.FileExistsMessage;
            string? replaceMessage = SettingsVM.Locale.ReplaceItMessage;

            QuickMessage quickMessage = new($"{fileExistsMessage}: {path}. {replaceMessage}?");
            ButtonResult msgResult = await quickMessage.ShowQuestionDialog(mainWindow);

            return msgResult == ButtonResult.Yes;
        }

        protected void Execute(ToolType toolType, YpdfConfig config, bool checkOutputPath)
        {
            if (!checkOutputPath)
            {
                Execute(toolType, config);
                return;
            }

            _ = VerifyOutputPath(config.PathsConfig.OutputPath).ContinueWith(t =>
            {
                if (t.Result)
                {
                    Execute(toolType, config);

                    if (SettingsVM.ResetAfterExecution)
                        Reset();
                }
                else
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        string? message = SettingsVM.Locale.FileExistsMessage;
                        string? path = config.PathsConfig.OutputPath;

                        MainWindowMessage.ShowErrorDialog($"{message}: {path}");
                    });
                }
            });
        }

        protected static Task<string[]?> GetPdfFilePath()
        {
            var dialog = new OpenFileDialog()
            {
                AllowMultiple = false,
                Title = "Select PDF file",

                Filters = new List<FileDialogFilter>()
                {
                    new FileDialogFilter() { Name = "PDF Documents", Extensions = new List<string>() { "pdf" } }
                }
            };

            return WindowFinder.FindMainWindow() is Window mainWindow
                ? dialog.ShowAsync(mainWindow)
                : new Task<string[]?>(() => null);
        }

        protected static Task<string?> GetDirectoryPath()
        {
            var dialog = new OpenFolderDialog()
            {
                Title = "Select output directory path"
            };

            return WindowFinder.FindMainWindow() is Window mainWindow
                ? dialog.ShowAsync(mainWindow)
                : new Task<string?>(() => null);
        }

        #endregion

        #region Private Methods

        private void Execute(ToolType toolType, YpdfConfig config)
        {
            var executor = new PdfToolExecutor(config);

            string[] inputFiles = config.PathsConfig.AllInputFiles
                .Select(t => Path.GetFileName(t))
                .ToArray();

            string? toolIcon = ToolInfoService.GetIconName(toolType);
            string? toolName = ToolInfoService.GetToolName(toolType, SettingsVM.Locale);

            Task executionTask = new Task(() =>
            {
                IExecutionInfo executionInfo = executor.PrepareExecute(false, false);
                executor.Execute(executionInfo);
            });

            var taskExecutionInfo = new ToolExecutionInfo(toolName, toolIcon, inputFiles, executionTask, ToolExecutionStatus.Running)
            {
                StatusBrush = ToolInfoService.GetExecutionStatusColor(ToolExecutionStatus.Running, SettingsVM.Theme)
            };

            taskExecutionInfo.StatusChanged += newStatus =>
            {
                taskExecutionInfo.StatusBrush = ToolInfoService.GetExecutionStatusColor(newStatus, SettingsVM.Theme);
            };

            TasksVM.Tasks.Insert(0, taskExecutionInfo);

            var textWriter = new ToolOutputWriter(taskExecutionInfo);
            executor.Logger = new ExecutionLogger(textWriter);

            executor.ExecutionFaulted += () =>
            {
                taskExecutionInfo.MakeFaulted();

                TasksVM.RunningTasksCount--;
                TasksVM.FaultedTasksCount++;
            };

            executor.ExecutionSuccessfullyCompleted += () =>
            {
                taskExecutionInfo.MakeCompleted();

                TasksVM.RunningTasksCount--;
                TasksVM.CompletedTasksCount++;
            };

            executionTask.Start();
        }

        #endregion
    }
}
