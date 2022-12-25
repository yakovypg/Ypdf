using System.Diagnostics;

namespace RuntimeLib.Processes
{
    public abstract class ProcessExecutor : ProcessController, IProcessExecutor
    {
        public abstract void Execute(string args);
        public abstract void Execute(string args, string workingDirectory);

        public ProcessExecutor(bool redirectStandardError = true, bool redirectStandardOutput = true, TextWriter? output = null)
            : base(redirectStandardError, redirectStandardOutput, output)
        {
        }

        protected void ExecuteProcess(Process process)
        {
            if (Output is null || (!RedirectStandardError && !RedirectStandardOutput))
            {
                process.Start();
                process.WaitForExit();
                return;
            }

            using (var errorWaitHandle = new AutoResetEvent(false))
            using (var outputWaitHandle = new AutoResetEvent(false))
            {
                if (RedirectStandardError)
                {
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                            errorWaitHandle.Set();
                        else
                            WriteRecievedErrorData(e.Data);
                    };
                }

                if (RedirectStandardOutput)
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                            outputWaitHandle.Set();
                        else
                            WriteRecievedOutputData(e.Data);
                    };
                }

                process.Start();

                if (RedirectStandardError)
                    process.BeginErrorReadLine();

                if (RedirectStandardOutput)
                    process.BeginOutputReadLine();

                process.WaitForExit();

                if (RedirectStandardError)
                    errorWaitHandle.WaitOne();

                if (RedirectStandardOutput)
                    outputWaitHandle.WaitOne();
            }
        }

        protected virtual void WriteRecievedErrorData(string? data, bool addNewLine = true)
        {
            if (ErrorDataVerifier is not null && !ErrorDataVerifier.Invoke(data))
                return;

            if (ErrorDataConverter is not null)
                data = ErrorDataConverter.Invoke(data);

            Output?.Write(data);

            if (addNewLine)
                Output?.WriteLine();
        }

        protected virtual void WriteRecievedOutputData(string? data, bool addNewLine = true)
        {
            if (OutputDataVerifier is not null && !OutputDataVerifier.Invoke(data))
                return;

            if (OutputDataConverter is not null)
                data = OutputDataConverter.Invoke(data);

            Output?.Write(data);

            if (addNewLine)
                Output?.WriteLine();
        }
    }
}
