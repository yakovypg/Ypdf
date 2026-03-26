using System.Collections.Generic;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Runtime.Python;

namespace Ypdf.Core.Tools;

public abstract class PythonTool : ITool
{
    protected PythonTool(
        string? pythonAlias = null,
        string? virtualEnvironmentPath = null,
        IOutputWriter? outputWriter = null)
    {
        PythonAlias = pythonAlias;
        VirtualEnvironmentPath = virtualEnvironmentPath;
        OutputWriter = outputWriter;
    }

    protected string? PythonAlias { get; }
    protected string? VirtualEnvironmentPath { get; }
    protected IOutputWriter? OutputWriter { get; }
    protected abstract IEnumerable<PythonPackage> VirtualEnvironmentPackages { get; }

    public abstract void Execute(string inputPath, string outputPath);

    protected PythonVirtualEnvironmentConfig? CreatePythonVirtualEnvironmentConfig()
    {
        return !string.IsNullOrWhiteSpace(VirtualEnvironmentPath)
            ? new PythonVirtualEnvironmentConfig(null, VirtualEnvironmentPath!, [.. VirtualEnvironmentPackages])
            : null;
    }

    protected PythonConfig CreatePythonConfig()
    {
        PythonVirtualEnvironmentConfig? virtualEnvironment = CreatePythonVirtualEnvironmentConfig();
        return new PythonConfig(PythonAlias, true, virtualEnvironment);
    }

    protected PythonExecutor CreateDefaultPythonExecutor()
    {
        PythonConfig pythonConfig = CreatePythonConfig();

        return new PythonExecutor(pythonConfig)
        {
            OutputWriter = OutputWriter,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            ThrowExceptionIfExitWithError = true
        };
    }

    protected void ExecutePython(string args)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(args, nameof(args));

        PythonExecutor executor = CreateDefaultPythonExecutor();
        executor.Execute(args);
    }
}
