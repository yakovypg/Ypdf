namespace ExecutionLib.Informing.Logging
{
    public interface IExecutionLogger
    {
        TextWriter? Out { get; }

        void Log(string? text);
        void LogError(string? text);
        void LogInfo(string? text);
        void LogResult(string? text);
        void LogWarning(string? text);
    }
}