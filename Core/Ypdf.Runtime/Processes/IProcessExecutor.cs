namespace Ypdf.Runtime.Processes;

public interface IProcessExecutor : IProcessController
{
    void Execute(string args);
    void Execute(string args, string workingDirectory);
}
