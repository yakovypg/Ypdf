namespace RuntimeLib.Processes
{
    public abstract class ProcessController : IProcessController
    {
        public bool RedirectStandardError { get; set; }
        public bool RedirectStandardOutput { get; set; }
        public bool ThrowExceptionIfExitWithError { get; set; }

        public TextWriter? Output { get; set; }

        public Func<string?, string?>? ErrorDataConverter { get; set; }
        public Func<string?, string?>? OutputDataConverter { get; set; }

        public Predicate<string?>? ErrorDataVerifier { get; set; }
        public Predicate<string?>? OutputDataVerifier { get; set; }

        public ProcessController(bool redirectStandardError = true, bool redirectStandardOutput = true,
            TextWriter? output = null, bool throwExceptionIfExitWithError = true)
        {
            RedirectStandardError = redirectStandardError;
            RedirectStandardOutput = redirectStandardOutput;
            ThrowExceptionIfExitWithError = throwExceptionIfExitWithError;
            Output = output;
        }
    }
}
