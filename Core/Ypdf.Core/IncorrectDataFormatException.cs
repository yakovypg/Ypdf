using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core;

[Serializable]
public class IncorrectDataFormatException : Exception
{
    public IncorrectDataFormatException() { }

    public IncorrectDataFormatException(string? message)
        : base(message) { }

    public IncorrectDataFormatException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public IncorrectDataFormatException(
        string? message,
        string receivedData,
        string expectedFormat)
        : this(message, receivedData, expectedFormat, null) { }

    public IncorrectDataFormatException(
        string? message,
        string receivedData,
        string expectedFormat,
        Exception? innerException)
        : base(message ?? GetDefaultMessage(receivedData, expectedFormat), innerException)
    {
        ReceivedData = receivedData;
        ExpectedFormat = expectedFormat;
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected IncorrectDataFormatException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        ReceivedData = info.GetString(nameof(ReceivedData));
        ExpectedFormat = info.GetString(nameof(ExpectedFormat));
    }

    public string? ReceivedData { get; private set; }
    public string? ExpectedFormat { get; private set; }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        info.AddValue(nameof(ReceivedData), ReceivedData, typeof(string));
        info.AddValue(nameof(ExpectedFormat), ExpectedFormat, typeof(string));

        base.GetObjectData(info, context);
    }

    private static string GetDefaultMessage(string receivedData, string expectedFormat)
    {
        ExtendedArgumentNullException.ThrowIfNull(receivedData, nameof(receivedData));
        ExtendedArgumentNullException.ThrowIfNull(expectedFormat, nameof(expectedFormat));

        return $"Received data '{receivedData}' has incorrect format. " +
                "Expected format: {expectedFormat}.";
    }
}
