using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace YpdfDesktop.Models.Informing
{
    public interface ITaskExecutionInfo
    {
        string? ToolName { get; }
        string? ToolIcon { get; }
        string ToolOutput { get; set; }

        Task ExecutionTask { get; }
        ObservableCollection<string> InputFiles { get; }
    }
}