using System.IO;
using System.Reflection;

namespace Ypdf.Core.Config;

public static class CoreDirectories
{
    static CoreDirectories()
    {
        _ = TryGetAssemblyLocation(out string assemblyLocation);
        _ = TryGetCurrentDirectory(out string currentDirectory, ".");

        AssemblyLocation = assemblyLocation;
        RootDirectory = GetRootDirectory(AssemblyLocation, currentDirectory);
        Temp = GetTempPath(RootDirectory, RootDirectory);
        Scripts = GetScriptsPath(RootDirectory);
    }

    public static string AssemblyLocation { get; }
    public static string RootDirectory { get; }

    public static string Temp { get; }
    public static string Scripts { get; }

    public static void Prepare()
    {
        PrepareDirectory(Temp);
        PrepareDirectory(Scripts);
    }

    private static void PrepareDirectory(string path)
    {
        if (string.IsNullOrEmpty(path) || Directory.Exists(path))
            return;

        Directory.CreateDirectory(path);
    }

    private static bool TryGetAssemblyLocation(out string assemblyLocation)
    {
        try
        {
            assemblyLocation = Assembly.GetExecutingAssembly().Location ?? string.Empty;
            return true;
        }
        catch
        {
            assemblyLocation = string.Empty;
            return false;
        }
    }

    private static bool TryGetCurrentDirectory(out string currentDirectory, string defaultValue = ".")
    {
        ExtendedArgumentNullException.ThrowIfNull(defaultValue, nameof(defaultValue));

        try
        {
            currentDirectory = Directory.GetCurrentDirectory() ?? defaultValue;
            return true;
        }
        catch
        {
            currentDirectory = defaultValue;
            return false;
        }
    }

    private static string GetRootDirectory(string assemblyLocation, string defaultValue = ".")
    {
        ExtendedArgumentNullException.ThrowIfNull(assemblyLocation,  nameof(assemblyLocation));
        ExtendedArgumentNullException.ThrowIfNull(defaultValue, nameof(defaultValue));

        try
        {
            return Path.GetDirectoryName(assemblyLocation) ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    private static string GetTempPath(string rootDirectory, string defaultValue = ".")
    {
        ExtendedArgumentNullException.ThrowIfNull(rootDirectory, nameof(rootDirectory));
        ExtendedArgumentNullException.ThrowIfNull(defaultValue, nameof(defaultValue));

        if (TryGetUserTempPath(out string userTempPath))
            return userTempPath;

        if (TryGetLocalTempPath(rootDirectory, out string localTempPath))
            return localTempPath;

        return defaultValue;
    }

    private static bool TryGetUserTempPath(out string userTempPath)
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

    private static bool TryGetLocalTempPath(string rootDirectory, out string localTempPath)
    {
        ExtendedArgumentNullException.ThrowIfNull(rootDirectory, nameof(rootDirectory));

        try
        {
            localTempPath = Path.Combine(rootDirectory, "Temp");
            return true;
        }
        catch
        {
            localTempPath = string.Empty;
            return false;
        }
    }

    private static string GetScriptsPath(string rootDirectory, string defaultValue = ".")
    {
        ExtendedArgumentNullException.ThrowIfNull(rootDirectory, nameof(rootDirectory));
        ExtendedArgumentNullException.ThrowIfNull(defaultValue, nameof(defaultValue));

        try
        {
            return Path.Combine(rootDirectory, "Scripts");
        }
        catch
        {
            return defaultValue;
        }
    }
}
