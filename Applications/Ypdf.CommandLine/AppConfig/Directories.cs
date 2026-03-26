using System.IO;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Config;

namespace Ypdf.CommandLine.AppConfig;

internal static class Directories
{
    private const string _configDirectoryName = "Config";
    private const string _defaultVirtualEnvironmentDirectoryName = ".venv";

    static Directories()
    {
        Config = GetConfigPath(CoreDirectories.UserLibConfig);
        DefaultVirtualEnvironment = GetDefaultVirtualEnvironmentPath(CoreDirectories.UserLibConfig);
    }

    internal static string Config { get; }
    internal static string DefaultVirtualEnvironment { get; }

    internal static void Prepare()
    {
        CoreDirectories.Prepare();
        CoreDirectories.PrepareDirectory(Config);
    }

    internal static bool TryPrepare()
    {
        try
        {
            Prepare();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string GetConfigPath(string userConfigDirectoryPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(userConfigDirectoryPath, nameof(userConfigDirectoryPath));

        try
        {
            return Path.Combine(userConfigDirectoryPath, _configDirectoryName);
        }
        catch
        {
            return $"{userConfigDirectoryPath}/{_configDirectoryName}";
        }
    }

    private static string GetDefaultVirtualEnvironmentPath(string userConfigDirectoryPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(userConfigDirectoryPath, nameof(userConfigDirectoryPath));

        try
        {
            return Path.Combine(userConfigDirectoryPath, _defaultVirtualEnvironmentDirectoryName);
        }
        catch
        {
            return $"{userConfigDirectoryPath}/{_defaultVirtualEnvironmentDirectoryName}";
        }
    }
}
