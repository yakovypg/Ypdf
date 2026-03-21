using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core.Security;

[Serializable]
public class NoPasswordSpecifiedException : Exception
{
    public NoPasswordSpecifiedException()
        : this(GetDefaultMessage()) { }

    public NoPasswordSpecifiedException(string? message)
        : base(message ?? GetDefaultMessage()) { }

    public NoPasswordSpecifiedException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), innerException) { }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected NoPasswordSpecifiedException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    private static string GetDefaultMessage()
    {
        return "At least one password must be specified.";
    }
}
