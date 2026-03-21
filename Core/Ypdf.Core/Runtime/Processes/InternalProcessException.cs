using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core.Runtime.Processes;

[Serializable]
public class InternalProcessException : Exception
{
    public InternalProcessException()
        : this(GetDefaultMessage()) { }

    public InternalProcessException(string? message)
        : base(message ?? GetDefaultMessage()) { }

    public InternalProcessException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), innerException) { }

    public InternalProcessException(
        string? message,
        string processName,
        int exitCode)
        : this(message ?? GetDefaultMessage(processName, exitCode), processName, exitCode, null) { }

    public InternalProcessException(
        string? message,
        string processName,
        int exitCode,
        Exception? innerException)
        : base(message ?? GetDefaultMessage(processName, exitCode), innerException)
    {
        ExtendedArgumentNullException.ThrowIfNull(processName, nameof(processName));

        ProcessName = processName;
        ExitCode = exitCode;
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected InternalProcessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        ProcessName = info.GetString(nameof(ProcessName));
        ExitCode = info.GetInt32(nameof(ExitCode));
    }

    public string? ProcessName { get; private set; }
    public int ExitCode { get; private set; }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        info.AddValue(nameof(ProcessName), ProcessName, typeof(string));
        info.AddValue(nameof(ExitCode), ExitCode);

        base.GetObjectData(info, context);
    }

    private static string GetDefaultMessage(string? processName = null)
    {
        if (!string.IsNullOrEmpty(processName))
            processName = $" {processName}";

        return $"Process{processName} ended with error.";
    }

    private static string GetDefaultMessage(string? processName, int exitCode)
    {
        string message = GetDefaultMessage(processName);
        return $"{message} Exit code: {exitCode}.";
    }
}
