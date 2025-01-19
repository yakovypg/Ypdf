using System;
using System.Diagnostics;
using System.Threading;

namespace Ypdf.Runtime.Processes;

public abstract class ProcessExecutor : ProcessController, IProcessExecutor
{
    protected ProcessExecutor() { }

    public abstract void Execute(string args);
    public abstract void Execute(string args, string workingDirectory);

    protected void ExecuteProcess(Process process)
    {
        if (process is null)
            throw new ArgumentNullException(nameof(process));

        if (OutputWriter is null || (!RedirectStandardError && !RedirectStandardOutput))
        {
            process.Start();
            process.WaitForExit();
            return;
        }

        using var errorWaitHandle = new AutoResetEvent(false);
        using var outputWaitHandle = new AutoResetEvent(false);

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

        if (process.ExitCode != 0 && ThrowExceptionIfExitWithError)
            throw new InternalProcessException(null, process.ProcessName, process.ExitCode);
    }

    protected virtual void WriteRecievedErrorData(string? data, bool addNewLine = true)
    {
        if (ErrorDataVerifier is not null && !ErrorDataVerifier.Invoke(data))
            return;

        if (ErrorDataConverter is not null)
            data = ErrorDataConverter.Invoke(data);

        OutputWriter?.Write(data);

        if (addNewLine)
            OutputWriter?.WriteLine();
    }

    protected virtual void WriteRecievedOutputData(string? data, bool addNewLine = true)
    {
        if (OutputDataVerifier is not null && !OutputDataVerifier.Invoke(data))
            return;

        if (OutputDataConverter is not null)
            data = OutputDataConverter.Invoke(data);

        OutputWriter?.Write(data);

        if (addNewLine)
            OutputWriter?.WriteLine();
    }
}
