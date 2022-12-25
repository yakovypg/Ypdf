namespace ExecutionLib.Informing.Logging
{
    public class ExecutionLogger : IExecutionLogger
    {
        public TextWriter Out { get; }

        public ExecutionLogger(TextWriter writer)
        {
            Out = writer;
        }

        public virtual void Log(string? text)
        {
            Out.WriteLine(text);
        }

        public virtual void LogInfo(string? text)
        {
            Out.WriteLine(text);
        }

        public virtual void LogResult(string? text)
        {
            Out.WriteLine(text);
        }

        public virtual void LogError(string? text)
        {
            Out.WriteLine(text);
        }

        public virtual void LogWarning(string? text)
        {
            Out.WriteLine(text);
        }
    }
}
