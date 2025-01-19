using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Runtime.Processes;

[Serializable]
public class InternalProcessException : Exception
{
    public InternalProcessException() { }

    public InternalProcessException(string? message)
        : base(message) { }

    public InternalProcessException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public InternalProcessException(
        string? message,
        string processName,
        int exitCode)
        : this(message, processName, exitCode, null) { }

    public InternalProcessException(
        string? message,
        string processName,
        int exitCode,
        Exception? innerException)
        : base(message ?? GetDefaultMessage(exitCode), innerException)
    {
        ProcessName = processName
            ?? throw new ArgumentNullException(nameof(processName));

        ExitCode = exitCode;
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected InternalProcessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        if (info is null)
            throw new ArgumentNullException(nameof(info));

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
        if (info is null)
            throw new ArgumentNullException(nameof(info));

        info.AddValue(nameof(ProcessName), ProcessName, typeof(string));
        info.AddValue(nameof(ExitCode), ExitCode);

        base.GetObjectData(info, context);
    }

    private static string GetDefaultMessage(int exitCode)
    {
        return $"Process ended with error (exit code: {exitCode}).";
    }
}
