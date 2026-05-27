using System;
using System.IO;
using System.Reflection;

namespace Ypdf.Core.Config;

public static class CoreDirectories
{
    private const string _currentDirectoryName = ".";
    private const string _userLibConfigDirectoryName = ".ypdf";
    private const string _localTempDirectoryName = "Temp";
    private const string _scriptsDirectoryName = "Scripts";

    static CoreDirectories()
    {
        AssemblyLocation = GetAssemblyLocationPath();
        CurrentDirectory = GetCurrentDirectoryPath();
        RootDirectory = GetRootDirectoryPath();
        UserDirectory = GetUserDirectoryPathOrDefault(RootDirectory);

        UserLibConfig = GetUserLibConfigPath(UserDirectory);
        TempDirectory = GetTempDirectoryPath(UserLibConfig);
        Scripts = GetScriptsPath(RootDirectory);
    }

    public static string AssemblyLocation { get; }
    public static string CurrentDirectory { get; }
    public static string RootDirectory { get; }
    public static string UserDirectory { get; }

    public static string UserLibConfig { get; }
    public static string TempDirectory { get; }
    public static string Scripts { get; }

    public static void Prepare()
    {
        PrepareDirectory(UserLibConfig);
        PrepareDirectory(TempDirectory);
        PrepareDirectory(Scripts);
    }

    public static bool TryPrepare()
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

    public static void PrepareDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || Directory.Exists(path))
            return;

        Directory.CreateDirectory(path);
    }

    public static string GetAssemblyLocationPath()
    {
        return Assembly.GetExecutingAssembly().Location;
    }

    public static string GetCurrentDirectoryPath()
    {
        try
        {
            return Directory.GetCurrentDirectory();
        }
        catch
        {
            return _currentDirectoryName;
        }
    }

    public static string GetRootDirectoryPath()
    {
        try
        {
            string assemblyLocation = GetAssemblyLocationPath();
            return Path.GetDirectoryName(assemblyLocation) ?? GetCurrentDirectoryPath();
        }
        catch
        {
            return GetCurrentDirectoryPath();
        }
    }

    public static bool TryGetSpecialFolderPath(
        Environment.SpecialFolder specialFolder,
        out string specialFolderPath)
    {
        try
        {
            specialFolderPath = Environment.GetFolderPath(specialFolder);
            return true;
        }
        catch
        {
            specialFolderPath = string.Empty;
            return false;
        }
    }

    public static string GetUserDirectoryPathOrDefault(string defaultPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(defaultPath, nameof(defaultPath));

        if (TryGetSpecialFolderPath(Environment.SpecialFolder.UserProfile, out string userProfile))
            return userProfile;

        if (TryGetSpecialFolderPath(Environment.SpecialFolder.MyDocuments, out string documents))
            return documents;

        return defaultPath;
    }

    public static string GetUserLibConfigPath(string userDirectoryPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(userDirectoryPath, nameof(userDirectoryPath));

        try
        {
            return Path.Combine(userDirectoryPath, _userLibConfigDirectoryName);
        }
        catch
        {
            return $"{userDirectoryPath}/{_userLibConfigDirectoryName}";
        }
    }

    public static bool TryGetUserTempPath(out string userTempPath)
    {
        try
        {
            userTempPath = Path.GetTempPath();
            return true;
        }
        catch
        {
            userTempPath = string.Empty;
            return false;
        }
    }

    private static string GetLocalTempPath(string userConfigDirectoryPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(userConfigDirectoryPath, nameof(userConfigDirectoryPath));

        try
        {
            return Path.Combine(userConfigDirectoryPath, _localTempDirectoryName);
        }
        catch
        {
            return $"{userConfigDirectoryPath}/{_localTempDirectoryName}";
        }
    }

    private static string GetTempDirectoryPath(string userConfigDirectoryPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(userConfigDirectoryPath, nameof(userConfigDirectoryPath));

        return TryGetUserTempPath(out string userTempPath)
            ? userTempPath
            : GetLocalTempPath(userConfigDirectoryPath);
    }

    private static string GetScriptsPath(string rootDirectory)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory, nameof(rootDirectory));

        try
        {
            return Path.Combine(rootDirectory, _scriptsDirectoryName);
        }
        catch
        {
            return $"{rootDirectory}/{_scriptsDirectoryName}";
        }
    }
}
