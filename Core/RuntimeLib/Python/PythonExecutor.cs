using RuntimeLib.Processes;
using System.Diagnostics;
using System.Reflection;

namespace RuntimeLib.Python
{
    public class PythonExecutor : ProcessExecutor
    {
        public PythonExecutor(bool redirectStandardError = true, bool redirectStandardOutput = true, TextWriter? output = null)
            : base(redirectStandardError, redirectStandardOutput, output)
        {
        }

        public override void Execute(string args)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string workingDirectory = Path.GetDirectoryName(assemblyLocation) ?? Directory.GetCurrentDirectory();

            Execute(args, workingDirectory);
        }

        public override void Execute(string args, string workingDirectory)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = "py",
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
    }
}
