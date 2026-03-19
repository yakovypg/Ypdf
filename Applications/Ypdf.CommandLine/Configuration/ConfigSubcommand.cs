using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

internal sealed class ConfigSubcommand
{
    internal const string Name = "config";
    internal const string Description = "Change global configuration";

    internal const string ShowConfigLongName = "show";
    internal const string SaveConfigLongName = "save";
    internal const string ResetConfigLongName = "reset";
    internal const string PythonAliasLongName = "python-alias";

    [FlagOption(
        longName: ShowConfigLongName,
        shortName: "",
        description: "show configuration")
    ]
    [MutuallyExclusiveOptionGroup(
        $"{nameof(ConfigSubcommand)}.Action",
        "Actions",
        "You can use only one of the following actions: " +
            $"{nameof(ShowConfig)}, {nameof(SaveConfig)}, {nameof(ResetConfig)}")
    ]
    internal bool ShowConfig { get; set; }

    [FlagOption(
        longName: SaveConfigLongName,
        shortName: "",
        description: "save configuration")
    ]
    [MutuallyExclusiveOptionGroup($"{nameof(ConfigSubcommand)}.Action", "", "")]
    internal bool SaveConfig { get; set; }

    [FlagOption(
        longName: ResetConfigLongName,
        shortName: "",
        description: "reset configuration")
    ]
    [MutuallyExclusiveOptionGroup($"{nameof(ConfigSubcommand)}.Action", "", "")]
    internal bool ResetConfig { get; set; }

    [ValueOption<string>(
        longName: PythonAliasLongName,
        shortName: "i",
        description: "path to the input file",
        valueRestriction: "!nullOrWhiteSpace" +
            "\n?pthon alias mustn't be an empty string or a string consisting only of whitespace characters")
    ]
    [OptionGroup("variables", "Global Variables", "Options for configuring global variables")]
    internal string? PythonAlias { get; set; }
}
