using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core.Compression;

[Serializable]
public class CompressionException : Exception
{
    public CompressionException()
        : this(GetDefaultMessage()) { }

    public CompressionException(string? message)
        : base(message ?? GetDefaultMessage()) { }

    public CompressionException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), innerException) { }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected CompressionException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    private static string GetDefaultMessage()
    {
        return "File compression failed.";
    }
}
