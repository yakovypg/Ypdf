using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Ypdf.Runtime.Processes;

namespace Ypdf.Runtime.Python;

public class PythonExecutor : ProcessExecutor
{
    public PythonExecutor()
    {
        PythonDetector.DetectPythonAlias(out string alias);
        PythonAlias = alias;
    }

    public string? PythonAlias { get; set; }
    public bool RequirePython3 { get; set; }

    public override void Execute(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            throw new ArgumentException(
                $"{nameof(args)} cannot be an empty string.",
                nameof(args));
        }

        string assemblyLocation = Assembly.GetExecutingAssembly().Location;

        string workingDirectory = Path.GetDirectoryName(assemblyLocation)
            ?? Directory.GetCurrentDirectory();

        Execute(args, workingDirectory);
    }

    public override void Execute(string args, string workingDirectory)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            throw new ArgumentException(
                $"{nameof(args)} cannot be an empty string.",
                nameof(args));
        }

        if (workingDirectory is null)
            throw new ArgumentNullException(nameof(workingDirectory));

        VerifyPythonVersion();

        string? pythonAlias = PythonAlias;

        if (pythonAlias is null)
            PythonDetector.DetectPythonAlias(out pythonAlias);

        var startInfo = new ProcessStartInfo()
        {
            FileName = pythonAlias,
            Arguments = args,
            WorkingDirectory = workingDirectory,

            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = RedirectStandardError,
            RedirectStandardOutput = RedirectStandardOutput
        };

        using var process = new Process()
        {
            StartInfo = startInfo
        };

        ExecuteProcess(process);
    }

    private void VerifyPythonVersion()
    {
        if (RequirePython3 && !PythonDetector.IsPython3Installed(out string python3Version))
            throw new PythonVersionNotSuitableException(null, python3Version, "3.x");
        else if (!PythonDetector.IsPythonInstalled())
            throw new PythonNotInstalledException();
    }
}
