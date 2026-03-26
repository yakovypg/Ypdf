using System;
using System.IO;
using Newtonsoft.Json;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core.Runtime.Logging;
using Ypdf.Core.Runtime.Python;

namespace Ypdf.CommandLine.AppConfig;

internal sealed class GlobalConfig
{
    private string _pythonAlias;
    private string _virtualEnvironmentPath;

    public GlobalConfig()
    {
        _pythonAlias = GetDefaultPythonAlias();
        _virtualEnvironmentPath = GetDefaultVirtualEnvironmentPath();

        OutputWriter = new ConsoleWriter();
    }

    public string PythonAlias
    {
        get => _pythonAlias;
        set => _pythonAlias = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string VirtualEnvironmentPath
    {
        get => _virtualEnvironmentPath;
        set => _virtualEnvironmentPath = value ?? throw new ArgumentNullException(nameof(value));
    }

    [JsonIgnore]
    internal IOutputWriter OutputWriter { get; }

    internal static GlobalConfig Load(string filePath)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

        string json = File.ReadAllText(filePath);
        GlobalConfig config = JsonConvert.DeserializeObject<GlobalConfig>(json) ?? new GlobalConfig();

        Recover(config);
        return config;
    }

    internal static bool TryLoad(string filePath, out GlobalConfig config)
    {
        try
        {
            config = Load(filePath);
            return true;
        }
        catch
        {
            config = new GlobalConfig();
            return false;
        }
    }

    internal string Serialize()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    internal void Save(string filePath)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

        string json = Serialize();
        File.WriteAllText(filePath, json);
    }

    internal void Reset(GlobalConfig? other = null)
    {
        other ??= new GlobalConfig();

        PythonAlias = other.PythonAlias;
        VirtualEnvironmentPath = other.VirtualEnvironmentPath;
    }

    internal GlobalConfig Copy()
    {
        return new GlobalConfig()
        {
            PythonAlias = PythonAlias,
            VirtualEnvironmentPath = VirtualEnvironmentPath
        };
    }

    private static string GetDefaultPythonAlias()
    {
        PythonDetector.DetectPythonAlias(out string pythonAlias);
        return pythonAlias;
    }

    private static string GetDefaultVirtualEnvironmentPath()
    {
        return Directories.DefaultVirtualEnvironment;
    }

    private static void Recover(GlobalConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        if (string.IsNullOrWhiteSpace(config.PythonAlias))
            config.PythonAlias = GetDefaultPythonAlias();

        if (string.IsNullOrWhiteSpace(config.VirtualEnvironmentPath))
            config.VirtualEnvironmentPath = GetDefaultVirtualEnvironmentPath();
    }
}
