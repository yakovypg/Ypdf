using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core;

[Serializable]
public class ExtendedArgumentNullException : ArgumentNullException
{
    public ExtendedArgumentNullException() { }

    public ExtendedArgumentNullException(string? paramName)
        : base(paramName) { }

    public ExtendedArgumentNullException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public ExtendedArgumentNullException(string? paramName, string? message)
        : base(paramName, message) { }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected ExtendedArgumentNullException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    public static void ThrowIfNull<T>(T? argument, string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ThrowIfNull(info, nameof(info));
        base.GetObjectData(info, context);
    }
}
