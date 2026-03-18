using System.IO;
using Newtonsoft.Json;
using Ypdf.Core;
using Ypdf.Core.Runtime.Logging;

namespace Ypdf.CommandLine.AppConfig;

internal sealed class GlobalConfig
{
    static GlobalConfig()
    {
        Instance = new GlobalConfig();
    }

    public GlobalConfig()
    {
        OutputWriter = new ConsoleWriter();
    }

    internal static GlobalConfig Instance { get; }

    internal string? PythonAlias { get; set; }

    [JsonIgnore]
    internal IOutputWriter OutputWriter { get; }

    internal static GlobalConfig Load(string filePath)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<GlobalConfig>(json) ?? new GlobalConfig();
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

    internal void Save(string filePath)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    internal void Reset(GlobalConfig? other = null)
    {
        other ??= new GlobalConfig();
        PythonAlias = other.PythonAlias;
    }
}
