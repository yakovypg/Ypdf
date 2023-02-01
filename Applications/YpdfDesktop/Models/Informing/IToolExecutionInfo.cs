using Avalonia.Media;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YpdfDesktop.Models.Informing
{
    public interface IToolExecutionInfo
    {
        string? ToolName { get; }
        string? ToolIcon { get; }
        string ToolOutput { get; set; }

        Task ExecutionTask { get; }
        IEnumerable<string> InputFiles { get; }

        string InputFilesPresenter { get; }

        string? StatusIcon { get; }
        ToolExecutionStatus Status { get; }
        ISolidColorBrush? StatusBrush { get; set; }

        void MakeRunning();
        void MakeCompleted();
        void MakeFaulted();
    }
}