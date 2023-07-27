using Avalonia.Threading;
using DynamicData;
using ExecutionLib.Configuration;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using YpdfDesktop.Infrastructure.Communication;
using YpdfDesktop.Models;
using YpdfDesktop.Models.IO;
using YpdfDesktop.ViewModels.Base;

namespace YpdfDesktop.ViewModels.Pages.Tools
{
    public class MergeViewModel : PdfToolViewModel, IFilePathCollectionContainer
    {
        #region Commands

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteSelectedInputFilesCommand { get; }
        public ReactiveCommand<Unit, Unit> AddInputFilesCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectOutputFilePathCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateSelectedInputFilesCountCommand { get; }

        #endregion

        #region Reactive Properties

        private string _outputFilePath = string.Empty;
        public string OutputFilePath
        {
            get => _outputFilePath;
            private set
            {
                this.RaiseAndSetIfChanged(ref _outputFilePath, value);
                IsOutputFilePathSelected = !string.IsNullOrEmpty(value);
            }
        }

        private bool _isOutputFilePathSelected = false;
        public bool IsOutputFilePathSelected
        {
            get => _isOutputFilePathSelected;
            private set => this.RaiseAndSetIfChanged(ref _isOutputFilePathSelected, value);
        }

        private bool _isAnyInputFilesAdded = false;
        public bool IsAnyInputFilesAdded
        {
            get => _isAnyInputFilesAdded;
            private set => this.RaiseAndSetIfChanged(ref _isAnyInputFilesAdded, value);
        }

        private bool _isAnyInputFilesSelected = false;
        public bool IsAnyInputFilesSelected
        {
            get => _isAnyInputFilesSelected;
            private set => this.RaiseAndSetIfChanged(ref _isAnyInputFilesSelected, value);
        }

        private int _inputFilesCount = 0;
        public int InputFilesCount
        {
            get => _inputFilesCount;
            private set
            {
                this.RaiseAndSetIfChanged(ref _inputFilesCount, value);
                IsAnyInputFilesAdded = value > 0;
            }
        }

        private int _selectedInputFilesCount = 0;
        public int SelectedInputFilesCount
        {
            get => _selectedInputFilesCount;
            private set
            {
                this.RaiseAndSetIfChanged(ref _selectedInputFilesCount, value);
                IsAnyInputFilesSelected = value > 0;
            }
        }

        private IList _selectedInputFiles = new List<InputFile>();
        public IList SelectedInputFiles
        {
            get => _selectedInputFiles;
            set => this.RaiseAndSetIfChanged(ref _selectedInputFiles, value);
        }

        #endregion

        #region Observable Collections

        public ObservableCollection<InputFile> InputFiles { get; }

        #endregion

        // Constructor for Designer
        public MergeViewModel() : this(new SettingsViewModel(), new TasksViewModel())
        {
        }

        public MergeViewModel(SettingsViewModel settingsVM, TasksViewModel tasksVM) : base(settingsVM, tasksVM)
        {
            InputFiles = new ObservableCollection<InputFile>();
            InputFiles.CollectionChanged += InputFilesCollectionChanged;

            ExecuteCommand = ReactiveCommand.Create(Execute);
            ResetCommand = ReactiveCommand.Create(Reset);
            DeleteSelectedInputFilesCommand = ReactiveCommand.Create(DeleteSelectedInputFiles);
            AddInputFilesCommand = ReactiveCommand.Create(AddInputFiles);
            SelectOutputFilePathCommand = ReactiveCommand.Create(SelectOutputFilePath);
            UpdateSelectedInputFilesCountCommand = ReactiveCommand.Create(UpdateSelectedInputFilesCount);
        }

        #region Protected Methods

        protected override void Execute()
        {
            if (!VerifyOutputFilePath())
                return;

            var config = new YpdfConfig()
            {
                PdfTool = "merge"
            };

            config.PathsConfig.OutputPath = CorrectOutputFilePath(OutputFilePath, "pdf");

            foreach (var inputFile in InputFiles)
                config.PathsConfig.FilePaths.Add(inputFile.File.FullName);

            Execute(ToolType.Merge, config, true);
        }

        protected override void Reset()
        {
            InputFiles.Clear();
            OutputFilePath = string.Empty;
        }

        #endregion

        #region Private Methods

        private void InputFilesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            InputFilesCount = InputFiles.Count;
        }

        private void DeleteSelectedInputFiles()
        {
            InputFiles.RemoveMany(SelectedInputFiles.Cast<InputFile>());
        }

        private void UpdateSelectedInputFilesCount()
        {
            SelectedInputFilesCount = SelectedInputFiles.Count;
        }

        private void AddInputFiles()
        {
            _ = DialogProvider.GetPdfFilePaths(true).ContinueWith(t =>
            {
                if (t.Result is null || t.Result.Length == 0)
                    return;

                foreach (string path in t.Result)
                {
                    try
                    {
                        InputFiles.Add(new InputFile(path));
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.UIThread.Post(() => MainWindowMessage.ShowErrorDialog(ex.Message));
                    }
                }
            });
        }

        private void SelectOutputFilePath()
        {
            const string initialFileName = "Merged";

            _ = DialogProvider.GetOutputFilePath(initialFileName, DialogProvider.PdfFilters).ContinueWith(t =>
            {
                if (t.Result is null || string.IsNullOrEmpty(t.Result))
                    return;

                OutputFilePath = t.Result;
            });
        }

        private bool VerifyOutputFilePath()
        {
            return InformIfIncorrect(IsOutputFilePathSelected, SettingsVM.Locale.SpecifyOutputFilePathMessage);
        }

        #endregion
    }
}
