using System;
using Ypdf.CommandLine.AppConfig;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Tools;

internal sealed class ResetGlobalConfigTool : ITool
{
    private readonly GlobalConfig _currentGlobalConfig;
    private readonly GlobalConfig? _newGlobalConfig;

    internal ResetGlobalConfigTool(GlobalConfig currentGlobalConfig, GlobalConfig? newGlobalConfig = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(currentGlobalConfig, nameof(currentGlobalConfig));

        _currentGlobalConfig = currentGlobalConfig;
        _newGlobalConfig = newGlobalConfig;
    }

    public void Execute(string inputPath, string outputPath)
    {
        // Input path isn't used
        ExtendedArgumentNullException.ThrowIfNull(outputPath, nameof(outputPath));

        _currentGlobalConfig.Reset(_newGlobalConfig);
        _currentGlobalConfig.Save(outputPath);
    }
}
