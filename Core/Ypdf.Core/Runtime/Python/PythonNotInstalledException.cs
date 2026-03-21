using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core.Runtime.Python;

[Serializable]
public class PythonNotInstalledException : Exception
{
    public PythonNotInstalledException()
        : this(GetDefaultMessage()) { }

    public PythonNotInstalledException(string? message)
        : base(message ?? GetDefaultMessage()) { }

    public PythonNotInstalledException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), innerException) { }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected PythonNotInstalledException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    private static string GetDefaultMessage()
    {
        return "Python isn't installed.";
    }
}
