using NetArgumentParser.Attributes;

namespace Ypdf.CommandLine.Configuration;

internal sealed class RemovePasswordSubcommand
{
    internal const string Name = "remove-password";
    internal const string Description = "Remove password from PDF document";

    internal const string InputPathLongName = "input";
    internal const string OutputPathLongName = "output";
    internal const string CommonPasswordLongName = "password";
    internal const string UserPasswordLongName = "user-password";
    internal const string OwnerPasswordLongName = "owner-password";

    [ValueOption<string>(
        longName: InputPathLongName,
        shortName: "i",
        description: "path to the input file",
        isRequired: true,
        valueRestriction: "file pdf\n?input path must point to a .pdf file")
    ]
    [OptionGroup("paths", "Paths", "Options for configuring paths")]
    internal string InputPath { get; set; } = string.Empty;

    [ValueOption<string>(
        longName: OutputPathLongName,
        shortName: "o",
        description: "path to the output file",
        isRequired: true)
    ]
    [OptionGroup("paths", "", "")]
    internal string OutputPath { get; set; } = string.Empty;

    [ValueOption<string>(
        longName: CommonPasswordLongName,
        shortName: "c",
        description: "common password (sets the same user password and owner password)")
    ]
    [OptionGroup("passwords", "Passwords", "Options for configuring passwords")]
    [MutuallyExclusiveOptionGroup(
        $"{nameof(RemovePasswordSubcommand)}.PasswordTypes",
        "Password Types",
        "You can use only one of the following password types: " +
            $"{nameof(CommonPassword)}, {nameof(UserPassword)}, {nameof(OwnerPassword)}")
    ]
    internal string? CommonPassword { get; set; }

    [ValueOption<string>(
        longName: UserPasswordLongName,
        shortName: "u",
        description: "user password")
    ]
    [OptionGroup("passwords", "", "")]
    [MutuallyExclusiveOptionGroup($"{nameof(RemovePasswordSubcommand)}.PasswordTypes", "", "")]
    internal string? UserPassword { get; set; }

    [ValueOption<string>(
        longName: OwnerPasswordLongName,
        shortName: "O",
        description: "owner password")
    ]
    [OptionGroup("passwords", "", "")]
    [MutuallyExclusiveOptionGroup($"{nameof(RemovePasswordSubcommand)}.PasswordTypes", "", "")]
    internal string? OwnerPassword { get; set; }
}
