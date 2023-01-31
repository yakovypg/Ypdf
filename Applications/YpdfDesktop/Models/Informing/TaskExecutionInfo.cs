using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using YpdfDesktop.Models.Base;

namespace YpdfDesktop.Models.Informing
{
    public class TaskExecutionInfo : ReactiveModel, ITaskExecutionInfo
    {
        public string? ToolName { get; }
        public string? ToolIcon { get; }

        public Task ExecutionTask { get; }
        public ObservableCollection<string> InputFiles { get; }

        private string _toolOutput = string.Empty;
        public string ToolOutput
        {
            get => _toolOutput;
            set => RaiseAndSetIfChanged(ref _toolOutput, value);
        }

        public TaskExecutionInfo(string? toolName, string? toolIcon, IEnumerable<string> inputFiles, Task executionTask)
        {
            ToolName = toolName;
            ToolIcon = toolIcon;
            ExecutionTask = executionTask;
            InputFiles = new ObservableCollection<string>(inputFiles);
        }
    }
}
