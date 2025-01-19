using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Runtime.Python;

[Serializable]
public class PythonVersionNotSuitableException : Exception
{
    public PythonVersionNotSuitableException() { }

    public PythonVersionNotSuitableException(string? message)
        : base(message) { }

    public PythonVersionNotSuitableException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public PythonVersionNotSuitableException(
        string? message,
        string currentVersion,
        string requiredVersion)
        : this(message, currentVersion, requiredVersion, null) { }

    public PythonVersionNotSuitableException(
        string? message,
        string currentVersion,
        string requiredVersion,
        Exception? innerException)
        : base(message ?? GetDefaultMessage(currentVersion, requiredVersion), innerException)
    {
        CurrentVersion = currentVersion;
        RequiredVersion = requiredVersion;
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected PythonVersionNotSuitableException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        if (info is null)
            throw new ArgumentNullException(nameof(info));

        CurrentVersion = info.GetString(nameof(CurrentVersion));
        RequiredVersion = info.GetString(nameof(RequiredVersion));
    }

    public string? CurrentVersion { get; private set; }
    public string? RequiredVersion { get; private set; }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info is null)
            throw new ArgumentNullException(nameof(info));

        info.AddValue(nameof(CurrentVersion), CurrentVersion, typeof(string));
        info.AddValue(nameof(RequiredVersion), RequiredVersion, typeof(string));

        base.GetObjectData(info, context);
    }

    private static string GetDefaultMessage(string currentVersion, string requiredVersion)
    {
        if (currentVersion is null)
            throw new ArgumentNullException(nameof(currentVersion));

        if (requiredVersion is null)
            throw new ArgumentNullException(nameof(requiredVersion));

        return $"Version {currentVersion} of the python is not suitable. "
            + $"Required version is {requiredVersion}.";
    }
}
