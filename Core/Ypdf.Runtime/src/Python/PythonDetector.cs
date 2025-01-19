using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Ypdf.Extensions;

namespace Ypdf.Runtime.Python;

public static class PythonDetector
{
    private const string _defaultPythonAlias = "python3";
    private const string _defaultWindowsPythonAlias = "py";
    private const string _defaultLinuxPythonAlias = "python3";
    private const string _defaultMacOsPythonAlias = "python3";
    private const string _alternativePythonAlias = "python";

    public static string DefaultPythonAlias
    {
        get
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsLinux()
                ? _defaultLinuxPythonAlias
                : OperatingSystem.IsWindows()
                ? _defaultWindowsPythonAlias
                : OperatingSystem.IsMacOS()
                ? _defaultMacOsPythonAlias
                : _defaultPythonAlias;
#else
            return _defaultPythonAlias;
#endif
        }
    }

    public static bool TryGetPythonVersion(string pythonAlias, out string version)
    {
        if (string.IsNullOrEmpty(pythonAlias))
        {
            throw new ArgumentException(
                $"{nameof(pythonAlias)} cannot be an empty string.",
                nameof(pythonAlias));
        }

        var proccessOutput = new StringBuilder();

        var startInfo = new ProcessStartInfo()
        {
            FileName = pythonAlias,
            Arguments = "--version",

            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        };

        using var process = new Process()
        {
            StartInfo = startInfo
        };

        process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                proccessOutput.Append(e.Data);
        });

        process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                proccessOutput.Append(e.Data);
        });

        try
        {
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
                throw new PythonNotInstalledException();
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch
        {
            version = string.Empty;
            return false;
        }
#pragma warning restore CA1031 // Do not catch general exception types

        string outputStr = proccessOutput.ToString();
        int spaceIndex = outputStr.IndexOf(' ', StringComparison.InvariantCulture);

        version = spaceIndex <= 0 || spaceIndex >= outputStr.Length - 1
            ? outputStr
            : outputStr.Substring(spaceIndex + 1);

        return true;
    }

    public static bool DetectPythonAlias(out string alias)
    {
        string[] standardPythonAliases =
        [
            _defaultWindowsPythonAlias,
            _defaultLinuxPythonAlias,
            _defaultMacOsPythonAlias,
            _alternativePythonAlias
        ];

        foreach (string pythonAlias in standardPythonAliases)
        {
            if (TryGetPythonVersion(pythonAlias, out string version))
            {
                alias = pythonAlias;
                return true;
            }
        }

        alias = DefaultPythonAlias;
        return false;
    }

    public static bool IsPythonInstalled()
    {
        return IsPythonInstalled(out _);
    }

    public static bool IsPythonInstalled(out string version)
    {
        if (!DetectPythonAlias(out string alias))
        {
            version = string.Empty;
            return false;
        }

        return TryGetPythonVersion(alias, out version);
    }

    public static bool IsPython3Installed()
    {
        return IsPython3Installed(out _);
    }

    public static bool IsPython3Installed(out string currentVersion)
    {
        return IsPythonInstalled(out currentVersion) &&
                currentVersion.FirstOrDefault(char.IsDigit) == '3';
    }
}
