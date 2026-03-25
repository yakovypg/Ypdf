namespace Ypdf.Core.Runtime.Processes;

public interface IProcessExecutor : IProcessController
{
    ProcessResult Execute(string args);
    ProcessResult Execute(string args, string workingDirectory);
}
