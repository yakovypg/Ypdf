using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.CommandLine.Execution;

[Serializable]
internal sealed class ToolNotSpecifiedException : Exception
{
    internal ToolNotSpecifiedException()
        : this(GetDefaultMessage()) { }

    internal ToolNotSpecifiedException(string? message)
        : base(message ?? GetDefaultMessage()) { }

    internal ToolNotSpecifiedException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), innerException) { }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    internal ToolNotSpecifiedException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    private static string GetDefaultMessage()
    {
        return "Tool isn't specified.";
    }
}
