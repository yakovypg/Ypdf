using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData;
using ExecutionLib.Configuration;
using ExecutionLib.Execution;
using ExecutionLib.Informing.Logging;
using FileSystemLib.Naming;
using MessageBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        protected static bool InformIfIncorrect(bool isCorrect, string? message)
        {
            if (isCorrect)
                return true;

            MainWindowMessage.ShowInformationDialog(message + ".");
            return false;
        }

        protected static string CorrectOutputFilePath(string outputFilePath, string expectedExtension)
        {
            if (string.IsNullOrEmpty(outputFilePath))
                return outputFilePath;

            string expectedPathEnding = $".{expectedExtension}";

            return !outputFilePath.ToLower().EndsWith(expectedPathEnding)
                ? outputFilePath + expectedPathEnding
                : outputFilePath;
        }

        protected static bool IsPathToPdf(string path)
        {
            return path.ToLower().EndsWith("pdf");
        }

        protected static bool IsPathToTxt(string path)
        {
            return path.ToLower().EndsWith("txt");
        }

        protected static bool LocalizeCollectionItem<T>(ObservableCollection<T> collection,
            int itemIndex, T localizedValue)
        {
            if (collection.Count <= itemIndex)
                return false;

            if (!(collection[itemIndex]?.Equals(localizedValue) ?? false))
            {
                collection[itemIndex] = localizedValue;
                return true;
            }

            return false;
        }

        protected async Task<bool> CheckFileExists(string? path)
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

        protected void Execute(ToolType toolType, YpdfConfig config, bool checkOutputPathExists)
        {
            if (!checkOutputPathExists)
            {
                _ = Execute(toolType, config);
                PreformPostExecutionActions();
                return;
            }

            _ = CheckFileExists(config.PathsConfig.OutputPath).ContinueWith(t =>
            {
                if (t.Result)
                {
                    _ = Execute(toolType, config);
                    PreformPostExecutionActions();
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

        protected void ExecuteManyAsOne(ToolType toolType, YpdfConfig[] configs, string inputPath,
            string outputPath, bool checkOutputPathExists)
        {
            if (!checkOutputPathExists)
            {
                ExecuteManyAsOne(toolType, configs, inputPath, outputPath);
                PreformPostExecutionActions();
                return;
            }

            _ = CheckFileExists(outputPath).ContinueWith(t =>
            {
                if (t.Result)
                {
                    ExecuteManyAsOne(toolType, configs, inputPath, outputPath);
                    PreformPostExecutionActions();
                }
                else
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        string? message = SettingsVM.Locale.FileExistsMessage;
                        MainWindowMessage.ShowErrorDialog($"{message}: {outputPath}");
                    });
                }
            });
        }

        #endregion

        #region Private Methods

        private Task Execute(ToolType toolType, YpdfConfig config)
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

            return executionTask;
        }

        private void ExecuteManyAsOne(ToolType toolType, YpdfConfig[] configs, string inputPath, string outputPath)
        {
            ArgumentNullException.ThrowIfNull(configs, nameof(configs));

            if (configs.Length == 0)
                return;

            if (configs.Length == 1)
            {
                YpdfConfig config = configs[0];
                config.PathsConfig.InputPath = inputPath;
                config.PathsConfig.OutputPath = outputPath;

                _ = Execute(toolType, config);
                return;
            }

            GeneratePathsPipeline(configs, inputPath, outputPath, out List<string> tempFilePaths);

            Task tasksWaiter = Task.Run(() =>
            {
                for (int i = 0; i < configs.Length; ++i)
                {
                    Task task = Execute(toolType, configs[i]);
                    TasksVM.Tasks[0].InputFilesPresenter = inputPath;
                    task.Wait();
                }
            });

            _ = tasksWaiter.ContinueWith(t =>
            {
                var tasksToDelete = TasksVM.Tasks.Skip(1).Take(configs.Length - 1);
                TasksVM.Tasks.RemoveMany(tasksToDelete);
                
                foreach (string path in tempFilePaths)
                {
                    try
                    {
                        if (File.Exists(path))
                            File.Delete(path);
                    }
                    catch { }
                }
            });
        }

        private void PreformPostExecutionActions()
        {
            if (SettingsVM.ResetAfterExecution)
                Reset();
        }

        private static void GeneratePathsPipeline(YpdfConfig[] configs, string inputPath, string outputPath)
        {
            GeneratePathsPipeline(configs, inputPath, outputPath, out _);
        }

        private static void GeneratePathsPipeline(YpdfConfig[] configs, string inputPath,
            string outputPath, out List<string> tempFilePaths)
        {
            ArgumentNullException.ThrowIfNull(configs, nameof(configs));

            tempFilePaths = new List<string>();

            if (configs.Length == 0)
                return;
            
            for (int i = 0; i < configs.Length; ++i)
            {
                YpdfConfig currConfig = configs[i];
                YpdfConfig? prevConfig = i > 0 ? configs[i - 1] : null;
                
                string? extension = Path.GetExtension(currConfig.PathsConfig.OutputPath);

                if (string.IsNullOrEmpty(extension))
                    extension = "pdf";
                
                string tempFilePath = new UniqueFile(extension, SharedConfig.Directories.Temp).GetNext();
                tempFilePaths.Add(tempFilePath);

                currConfig.PathsConfig.InputPath = prevConfig?.PathsConfig.OutputPath;
                currConfig.PathsConfig.OutputPath = tempFilePath;
            }

            configs[0].PathsConfig.InputPath = inputPath;
            configs[^1].PathsConfig.OutputPath = outputPath;

            tempFilePaths.RemoveAt(tempFilePaths.Count - 1);
        }

        #endregion
    }
}
