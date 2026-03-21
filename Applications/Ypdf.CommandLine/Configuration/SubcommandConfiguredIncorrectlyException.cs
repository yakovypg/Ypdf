using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using NetArgumentParser.Subcommands;

namespace Ypdf.CommandLine.Configuration;

[Serializable]
internal sealed class SubcommandConfiguredIncorrectlyException : Exception
{
    internal SubcommandConfiguredIncorrectlyException()
        : this(GetDefaultMessage()) { }

    internal SubcommandConfiguredIncorrectlyException(string? message)
        : base(message ?? GetDefaultMessage()) { }

    internal SubcommandConfiguredIncorrectlyException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), innerException) { }

    internal SubcommandConfiguredIncorrectlyException(string? message, Subcommand subcommand)
        : this(message ?? GetDefaultMessage(subcommand), subcommand, null) { }

    internal SubcommandConfiguredIncorrectlyException(
        string? message,
        Subcommand subcommand,
        Exception? innerException)
        : base(message ?? GetDefaultMessage(subcommand), innerException)
    {
        Subcommand = subcommand;
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    private SubcommandConfiguredIncorrectlyException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        Subcommand = (Subcommand?)info.GetValue(nameof(Subcommand), typeof(Subcommand))
            ?? default;
    }

    internal Subcommand? Subcommand { get; private set; }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        info.AddValue(nameof(Subcommand), Subcommand, typeof(Subcommand));
        base.GetObjectData(info, context);
    }

    private static string GetDefaultMessage(Subcommand? subcommand = null)
    {
        string subcommandName = subcommand is not null
            ? $" {subcommand.Name}"
            : string.Empty;

        return $"Subcommand{subcommandName} is configured incorrectly.";
    }
}
