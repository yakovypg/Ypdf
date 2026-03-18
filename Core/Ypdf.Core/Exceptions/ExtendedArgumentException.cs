using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Ypdf.Core;

[Serializable]
public class ExtendedArgumentException : ArgumentException
{
    public ExtendedArgumentException() { }

    public ExtendedArgumentException(string? message)
        : base(message) { }

    public ExtendedArgumentException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public ExtendedArgumentException(string? message, string? paramName)
        : base(message, paramName) { }

    public ExtendedArgumentException(
        string? message,
        string? paramName,
        Exception? innerException)
        : base(message, paramName, innerException) { }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected ExtendedArgumentException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    public static void ThrowIfNullOrEmpty<T>(T? argument, string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);

        if (argument is string str && string.IsNullOrEmpty(str))
        {
            throw new ArgumentException(
                $"{paramName} cannot be an empty string.",
                paramName);
        }

        if (argument is IEnumerable enumerable && !enumerable.Cast<object>().Any())
        {
            throw new ArgumentException(
                $"{paramName} cannot be an empty collection.",
                paramName);
        }
    }

    public static void ThrowIfContainsNullOrEmptyItem(
        IEnumerable argument,
        string? paramName = null)
    {
        ExtendedArgumentNullException.ThrowIfNull(argument, nameof(argument));

        int index = 0;

        foreach (object? item in argument)
        {
            ThrowIfNullOrEmpty(item, $"{paramName}[{index}]");
            index++;
        }
    }

    public static void ThrowIfNullOrWhiteSpace<T>(T? argument, string? paramName = null)
    {
        if (argument is null)
            throw new ArgumentNullException(paramName);

        if (argument is not string str)
            throw new NotSupportedException($"Type {typeof(T)} isn't supported.");

        if (string.IsNullOrWhiteSpace(str))
        {
            throw new ArgumentException(
                $"{paramName} cannot be an empty string.",
                paramName);
        }
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));
        base.GetObjectData(info, context);
    }
}
