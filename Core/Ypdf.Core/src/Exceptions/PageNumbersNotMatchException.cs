using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core;

[Serializable]
public class PageNumbersNotMatchException : Exception
{
    public PageNumbersNotMatchException() { }

    public PageNumbersNotMatchException(string? message)
        : base(message) { }

    public PageNumbersNotMatchException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public PageNumbersNotMatchException(
        string? message,
        int expectedNumberOfPages,
        int actualNumberOfPages)
        : this(message, expectedNumberOfPages, actualNumberOfPages, null) { }

    public PageNumbersNotMatchException(
        string? message,
        int expectedNumberOfPages,
        int actualNumberOfPages,
        Exception? innerException)
        : base(message ?? GetDefaultMessage(expectedNumberOfPages, actualNumberOfPages), innerException)
    {
        ExpectedNumberOfPages = expectedNumberOfPages;
        ActualNumberOfPages = actualNumberOfPages;
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected PageNumbersNotMatchException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        if (info is null)
            throw new ArgumentNullException(nameof(info));

        ExpectedNumberOfPages = info.GetInt32(nameof(ExpectedNumberOfPages));
        ActualNumberOfPages = info.GetInt32(nameof(ActualNumberOfPages));
    }

    public int ExpectedNumberOfPages { get; private set; }
    public int ActualNumberOfPages { get; private set; }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info is null)
            throw new ArgumentNullException(nameof(info));

        info.AddValue(nameof(ExpectedNumberOfPages), ExpectedNumberOfPages);
        info.AddValue(nameof(ActualNumberOfPages), ActualNumberOfPages);

        base.GetObjectData(info, context);
    }

    private static string GetDefaultMessage(int expectedNumberOfPages, int actualNumberOfPages)
    {
        return
            $"The number of pages is incorrect. Actual number is {actualNumberOfPages}, " +
            $"but {expectedNumberOfPages} is expected.";
    }
}
