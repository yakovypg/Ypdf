using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core.Execution.Validation;

[Serializable]
public class ValidationException : Exception
{
    public ValidationException()
        : this(GetDefaultMessage()) { }

    public ValidationException(string? message)
        : base(message ?? GetDefaultMessage()) { }

    public ValidationException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), innerException) { }

    public ValidationException(string? message, ValidationError validationError)
        : this(message ?? GetDefaultMessage(validationError), validationError, null) { }

    public ValidationException(
        string? message,
        ValidationError validationError,
        Exception? innerException)
        : base(message ?? GetDefaultMessage(validationError), innerException)
    {
        ValidationError = validationError;
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    private ValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        ValidationError = (ValidationError?)info.GetValue(nameof(ValidationError), typeof(ValidationError))
            ?? default;
    }

    public ValidationError? ValidationError { get; private set; }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        info.AddValue(nameof(ValidationError), ValidationError, typeof(ValidationError));
        base.GetObjectData(info, context);
    }

    private static string GetDefaultMessage(ValidationError? validationError = null)
    {
        return validationError?.Reason ?? "Some validation failed.";
    }
}
