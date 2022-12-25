using System.Diagnostics;
using System.Text;

namespace RuntimeLib.Python
{
    public static class PythonDetector
    {
        public const string DEFAULT_PYTHON_ALIAS = "python3";
        public const string DEFAULT_WINDOWS_PYTHON_ALIAS = "py";
        public const string DEFAULT_LINUX_PYTHON_ALIAS = "python3";
        public const string DEFAULT_MAC_PYTHON_ALIAS = "python3";

        public static string[] StandardPythonAliases { get; } =
        {
            "py",
            "python3",
            "python"
        };

        public static bool TryGetPythonVersion(out string version, string pythonAlias = DEFAULT_PYTHON_ALIAS)
        {
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

            var process = new Process()
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
            catch
            {
                version = string.Empty;
                return false;
            }

            string outputStr = proccessOutput.ToString();
            int spaceIndex = outputStr.IndexOf(' ');

            version = spaceIndex <= 0 || spaceIndex >= outputStr.Length - 1
                ? outputStr
                : outputStr[(spaceIndex + 1)..];

            return true;
        }

        public static bool DetectPythonAlias(out string alias)
        {
            foreach (string pythonAlias in StandardPythonAliases)
            {
                if (TryGetPythonVersion(out string version, pythonAlias))
                {
                    alias = pythonAlias;
                    return true;
                }
            }

            alias = GetDefaultPythonAlias();
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

            return TryGetPythonVersion(out version, alias);
        }

        public static bool IsPython3Installed()
        {
            return IsPython3Installed(out _);
        }

        public static bool IsPython3Installed(out string currentVersion)
        {
            return IsPythonInstalled(out currentVersion) &&
                   currentVersion.FirstOrDefault(t => char.IsDigit(t)) == '3';
        }

        public static string GetDefaultPythonAlias()
        {
            return OperatingSystem.IsLinux()
                ? DEFAULT_LINUX_PYTHON_ALIAS
                : OperatingSystem.IsWindows()
                ? DEFAULT_WINDOWS_PYTHON_ALIAS
                : OperatingSystem.IsMacOS()
                ? DEFAULT_MAC_PYTHON_ALIAS
                : DEFAULT_PYTHON_ALIAS;
        }
    }
}
