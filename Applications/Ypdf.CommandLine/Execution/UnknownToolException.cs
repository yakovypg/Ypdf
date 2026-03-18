using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.CommandLine.Execution;

[Serializable]
internal sealed class UnknownToolException : Exception
{
    internal UnknownToolException() { }

    internal UnknownToolException(string? message)
        : base(message) { }

    internal UnknownToolException(string? message, Exception? innerException)
        : base(message, innerException) { }

    internal UnknownToolException(
        string? message,
        string toolName)
        : this(message, toolName, null) { }

    internal UnknownToolException(
        string? message,
        string toolName,
        Exception? innerException)
        : base(message ?? GetDefaultMessage(toolName), innerException)
    {
        ToolName = toolName;
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    internal UnknownToolException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));
        ToolName = info.GetString(nameof(ToolName));
    }

    internal string? ToolName { get; private set; }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        info.AddValue(nameof(ToolName), ToolName, typeof(string));
        base.GetObjectData(info, context);
    }

    private static string GetDefaultMessage(string toolName)
    {
        ExtendedArgumentNullException.ThrowIfNull(toolName, nameof(toolName));
        return $"Tool '{toolName}' is unknown.";
    }
}
