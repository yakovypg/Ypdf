using System.Diagnostics;
using Ypdf.Configuration;

namespace Ypdf.Runtime.Python
{
    internal class PythonExecutor : ProcessExecutor
    {
        public PythonExecutor(bool redirectStandardError = true, bool redirectStandardOutput = true, TextWriter? output = null)
            : base(redirectStandardError, redirectStandardOutput, output)
        {
        }

        public override void Execute(string args)
        {
            Execute(args, ToolInfo.WorkingDirectory);
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
