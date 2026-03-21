using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core.Runtime.Python;

[Serializable]
public class PythonVersionNotSuitableException : Exception
{
    public PythonVersionNotSuitableException()
        : this(GetDefaultMessage()) { }

    public PythonVersionNotSuitableException(string? message)
        : base(message ?? GetDefaultMessage()) { }

    public PythonVersionNotSuitableException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), innerException) { }

    public PythonVersionNotSuitableException(
        string? message,
        string currentVersion,
        string requiredVersion)
        : this(
            message ?? GetDefaultMessage(currentVersion, requiredVersion),
            currentVersion,
            requiredVersion,
            null) { }

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
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

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
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        info.AddValue(nameof(CurrentVersion), CurrentVersion, typeof(string));
        info.AddValue(nameof(RequiredVersion), RequiredVersion, typeof(string));

        base.GetObjectData(info, context);
    }

    private static string GetDefaultMessage(string? currentVersion = null, string? requiredVersion = null)
    {
        if (!string.IsNullOrEmpty(currentVersion))
            currentVersion = $" {currentVersion}";

        string message = $"Version{currentVersion} of the python is not suitable.";

        return !string.IsNullOrEmpty(requiredVersion)
            ? $"{message} Required version is {requiredVersion}."
            : message;
    }
}
