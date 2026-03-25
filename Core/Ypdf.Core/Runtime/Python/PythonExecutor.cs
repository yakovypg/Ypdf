using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Ypdf.Core.Config;
using Ypdf.Core.Extensions;
using Ypdf.Core.Runtime.Processes;

namespace Ypdf.Core.Runtime.Python;

public class PythonExecutor : ProcessExecutor
{
    private const string _defaultVirtualEnvironmentPath = ".venv";

    public PythonExecutor(PythonConfig pythonConfig)
    {
        ExtendedArgumentNullException.ThrowIfNull(pythonConfig, nameof(pythonConfig));
        PythonConfig = pythonConfig;
    }

    public PythonConfig PythonConfig { get; }

    protected bool ShouldUseVirtualEnvironment => !string.IsNullOrEmpty(PythonConfig.VirtualEnvironment?.Path);
    protected bool ShouldConfigureVirtualEnvironment => PythonConfig.VirtualEnvironment?.Packages.Count > 0;

    protected bool VirtualEnvironmentCreated =>
        !string.IsNullOrEmpty(PythonConfig.VirtualEnvironment?.Path) &&
        Directory.Exists(PythonConfig.VirtualEnvironment?.Path) &&
        File.Exists(GetPythonExecutablePathForVirtualEnvironment(PythonConfig.VirtualEnvironment?.Path!));

    public override ProcessResult Execute(string args)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(args, nameof(args));

        string assemblyLocation = CoreDirectories.AssemblyLocation;

        string workingDirectory = Path.GetDirectoryName(assemblyLocation)
            ?? Directory.GetCurrentDirectory();

        return Execute(args, workingDirectory);
    }

    public override ProcessResult Execute(string args, string workingDirectory)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(args, nameof(args));
        ExtendedArgumentNullException.ThrowIfNull(workingDirectory, nameof(workingDirectory));

        return ExecuteInternal(args, workingDirectory, PythonConfig.Alias, true);
    }

    protected virtual ProcessResult ExecuteInternal(
        string args,
        string workingDirectory,
        string? pythonToUse,
        bool useVirtualEnvironment)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(args, nameof(args));
        ExtendedArgumentNullException.ThrowIfNull(workingDirectory, nameof(workingDirectory));

        VerifyPythonVersion();

        if (useVirtualEnvironment && ShouldUseVirtualEnvironment)
        {
            EnsureVirtualEnvironmentCreated(workingDirectory);
            EnsureVirtualEnvironmentConfigured(workingDirectory);

            string venvPython = GetPythonExecutablePathForVirtualEnvironment(PythonConfig.VirtualEnvironment?.Path!);

            if (File.Exists(venvPython))
                pythonToUse = venvPython;
        }

        if (string.IsNullOrWhiteSpace(pythonToUse))
            _ = PythonDetector.DetectPythonAlias(out pythonToUse);

        var startInfo = new ProcessStartInfo()
        {
            FileName = pythonToUse,
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

        return new ProcessResult(pythonToUse!, process.ExitCode);
    }

    protected virtual void VerifyPythonVersion()
    {
        const string requiredVersion = "3.x";

        if (PythonConfig.RequirePython3 && !PythonDetector.IsPython3Installed(out string python3Version))
            throw new PythonVersionNotSuitableException(null, python3Version, requiredVersion);
        else if (!PythonDetector.IsPythonInstalled())
            throw new PythonNotInstalledException();
    }

    private static string GetPythonExecutablePathForVirtualEnvironment(string virtualEnvironmentPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(virtualEnvironmentPath, nameof(virtualEnvironmentPath));

        return PlatformTypeDetector.IsWindows
            ? Path.Combine(virtualEnvironmentPath, "Scripts", "python.exe")
            : Path.Combine(virtualEnvironmentPath, "bin", "python");
    }

    private static string CreateArgsForCreatingVirtualEnvironment(
        string? virtualEnvironmentPath = _defaultVirtualEnvironmentPath,
        string? virtualEnvironmentName = null)
    {
        virtualEnvironmentPath ??= _defaultVirtualEnvironmentPath;
        virtualEnvironmentPath = virtualEnvironmentPath.Quoted();

        string args = $"-m venv {virtualEnvironmentPath}";

        return !string.IsNullOrWhiteSpace(virtualEnvironmentName)
            ? $"{args} --prompt {virtualEnvironmentName}"
            : args;
    }

    private static string CreateArgsForInstallingPackages(IEnumerable<PythonPackage> packages)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(packages, nameof(packages));

        IEnumerable<string> packageNames = packages.Select(t => t.FullName);
        string joinedPackages = string.Join(" ", packageNames);

        return $"-m pip install {joinedPackages}";
    }

    private void CreateVirtualEnvironment(string workingDirectory)
    {
        ExtendedArgumentNullException.ThrowIfNull(workingDirectory, nameof(workingDirectory));

        OutputWriter?.WriteLine("Trying to create Python virtual environment.");

        string args = CreateArgsForCreatingVirtualEnvironment(
            PythonConfig.VirtualEnvironment?.Path,
            PythonConfig.VirtualEnvironment?.Name);

        ProcessResult createVirtualEnvironmentResult = ExecuteInternal(args, workingDirectory, PythonConfig.Alias, false);

        if (!createVirtualEnvironmentResult.IsSuccessful)
        {
            OutputWriter?.WriteLine("Failed to create Python virtual environment.");
            return;
        }

        OutputWriter?.WriteLine("Python virtual environment created.");
    }

    private void EnsureVirtualEnvironmentCreated(string workingDirectory)
    {
        ExtendedArgumentNullException.ThrowIfNull(workingDirectory, nameof(workingDirectory));

        if (!VirtualEnvironmentCreated)
            CreateVirtualEnvironment(workingDirectory);
    }

    private void EnsureVirtualEnvironmentConfigured(string workingDirectory)
    {
        ExtendedArgumentNullException.ThrowIfNull(workingDirectory, nameof(workingDirectory));

        if (ShouldConfigureVirtualEnvironment)
            InstallVirtualEnvironmentPackages(workingDirectory);
    }

    private void InstallVirtualEnvironmentPackages(string workingDirectory)
    {
        ExtendedArgumentNullException.ThrowIfNull(workingDirectory, nameof(workingDirectory));

        if (PythonConfig.VirtualEnvironment is null || PythonConfig.VirtualEnvironment.Packages.Count == 0)
            return;

        OutputWriter?.WriteLine("Trying to install required Python packages.");

        if (!VirtualEnvironmentCreated)
        {
            OutputWriter?.WriteLine("Failed to install required Python packages: virtual environment not present.");
            return;
        }

        string args = CreateArgsForInstallingPackages(PythonConfig.VirtualEnvironment.Packages);
        string venvPython = GetPythonExecutablePathForVirtualEnvironment(PythonConfig.VirtualEnvironment.Path);
        ProcessResult installPackagesResult = ExecuteInternal(args, workingDirectory, venvPython, false);

        if (!installPackagesResult.IsSuccessful)
        {
            OutputWriter?.WriteLine("Failed to install required Python packages.");
            return;
        }

        OutputWriter?.WriteLine("Required Python packages installed.");
    }
}
