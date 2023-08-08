using Avalonia.Media;
using System.Collections.Generic;
using System.Threading.Tasks;
using YpdfDesktop.Infrastructure.Services;
using YpdfDesktop.Models.Base;

namespace YpdfDesktop.Models.Informing
{
    public class ToolExecutionInfo : ReactiveModel, IToolExecutionInfo
    {
        public delegate void StatusChangedHandler(ToolExecutionStatus newValue);
        public event StatusChangedHandler? StatusChanged;

        public string? ToolName { get; }
        public string? ToolIcon { get; }

        public Task ExecutionTask { get; }
        public IEnumerable<string> InputFiles { get; }

        private string _inputFilesPresenter = string.Empty;
        public string InputFilesPresenter
        {
            get => _inputFilesPresenter;
            set => RaiseAndSetIfChanged(ref _inputFilesPresenter, value);
        }

        private string _toolOutput = string.Empty;
        public string ToolOutput
        {
            get => _toolOutput;
            set => RaiseAndSetIfChanged(ref _toolOutput, value);
        }

        private string? _statusIcon;
        public string? StatusIcon
        {
            get => _statusIcon;
            private set => RaiseAndSetIfChanged(ref _statusIcon, value);
        }

        private ToolExecutionStatus _status;
        public ToolExecutionStatus Status
        {
            get => _status;
            private set
            {
                if (!RaiseAndSetIfChanged(ref _status, value))
                    return;

                StatusIcon = ToolInfoService.GetExecutionStatusIconName(value);
                StatusChanged?.Invoke(value);
            }
        }

        private ISolidColorBrush? _statusBrush = Brushes.White;
        public ISolidColorBrush? StatusBrush
        {
            get => _statusBrush;
            set => RaiseAndSetIfChanged(ref _statusBrush, value);
        }

        public ToolExecutionInfo(string? toolName, string? toolIcon, IEnumerable<string> inputFiles, Task executionTask, ToolExecutionStatus taskStatus)
        {
            ToolName = toolName;
            ToolIcon = toolIcon;
            Status = taskStatus;
            ExecutionTask = executionTask;
            InputFiles = inputFiles;

            InputFilesPresenter = string.Join(", ", inputFiles);
            StatusIcon = ToolInfoService.GetExecutionStatusIconName(Status);
        }

        public void MakeRunning()
        {
            Status = ToolExecutionStatus.Completed;
        }

        public void MakeCompleted()
        {
            Status = ToolExecutionStatus.Completed;
        }

        public void MakeFaulted()
        {
            Status = ToolExecutionStatus.Faulted;
        }
    }
}
