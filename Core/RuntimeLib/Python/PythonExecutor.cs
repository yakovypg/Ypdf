using RuntimeLib.Processes;
using System.Diagnostics;
using System.Reflection;

namespace RuntimeLib.Python
{
    public class PythonExecutor : ProcessExecutor
    {
        public string PythonAlias { get; set; }
        public bool RequirePython3 { get; set; }
        public bool CheckPythonIsInstalled { get; set; }

        public PythonExecutor(bool redirectStandardError = true, bool redirectStandardOutput = true, TextWriter? output = null)
            : base(redirectStandardError, redirectStandardOutput, output)
        {
            PythonDetector.DetectPythonAlias(out string alias);
            PythonAlias = alias;
        }

        public override void Execute(string args)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string workingDirectory = Path.GetDirectoryName(assemblyLocation) ?? Directory.GetCurrentDirectory();

            Execute(args, workingDirectory);
        }

        public override void Execute(string args, string workingDirectory)
        {
            VerifyPythonVersion();

            var startInfo = new ProcessStartInfo()
            {
                FileName = PythonAlias,
                Arguments = args,
                WorkingDirectory = workingDirectory,

                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = RedirectStandardError,
                RedirectStandardOutput = RedirectStandardOutput
            };

            var process = new Process()
            {
                StartInfo = startInfo
            };

            ExecuteProcess(process);
        }

        private void VerifyPythonVersion()
        {
            if (!CheckPythonIsInstalled && !RequirePython3)
                return;

            if (RequirePython3 && !PythonDetector.IsPython3Installed(out string python3Version))
                throw new PythonNotInstalledException(python3Version, "3");

            else if (CheckPythonIsInstalled && !PythonDetector.IsPythonInstalled(out string pythonVersion))
                throw new PythonNotInstalledException(pythonVersion, null);
        }
    }
}
