using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core;

[Serializable]
internal sealed class ExtendedArgumentNullException : ArgumentNullException
{
    internal ExtendedArgumentNullException() { }

    internal ExtendedArgumentNullException(string? paramName)
        : base(paramName) { }

    internal ExtendedArgumentNullException(string? message, Exception? innerException)
        : base(message, innerException) { }

    internal ExtendedArgumentNullException(string? paramName, string? message)
        : base(paramName, message) { }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    private ExtendedArgumentNullException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ThrowIfNull(info, nameof(info));
        base.GetObjectData(info, context);
    }

    internal static void ThrowIfNull<T>(T? argument, string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);
    }
}
