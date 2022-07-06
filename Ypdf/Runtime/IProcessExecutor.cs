namespace Ypdf.Runtime
{
    internal interface IProcessExecutor : IProcessController
    {
        void Execute(string args);
        void Execute(string args, string workingDirectory);
    }
}
