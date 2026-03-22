using System.IO;
using Newtonsoft.Json;
using Ypdf.CommandLine.Exceptions;
using Ypdf.Core;
using Ypdf.Core.Runtime.Logging;

namespace Ypdf.CommandLine.AppConfig;

internal sealed class GlobalConfig
{
    public GlobalConfig()
    {
        OutputWriter = new ConsoleWriter();
    }

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
    }

    internal GlobalConfig Copy()
    {
        return new GlobalConfig()
        {
            PythonAlias = PythonAlias
        };
    }
}
