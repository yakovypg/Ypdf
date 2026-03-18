using System;
using Ypdf.Core.Runtime.Logging;

namespace Ypdf.CommandLine.Informing;

internal sealed class ErrorMessageWriter : ConsoleWriter
{
    internal ErrorMessageWriter(int defaultExitCode = 1, bool overwriteExitCode = false)
    {
        DefaultExitCode = defaultExitCode;
        OverwriteExitCode = overwriteExitCode;
    }

    internal int DefaultExitCode { get; init; }
    internal bool OverwriteExitCode { get; init; }

    public override void Write(string? text)
    {
        base.Write(text);
        CloseConsole();
    }

    public override void WriteLine(string? text = null)
    {
        base.WriteLine(text);
        CloseConsole();
    }

    private void CloseConsole()
    {
        if (OverwriteExitCode || Environment.ExitCode == 0)
            Environment.ExitCode = DefaultExitCode;

        Environment.Exit(Environment.ExitCode);
    }
}
