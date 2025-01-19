using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core.Security;

[Serializable]
public class IncorrectPasswordException : Exception
{
    public IncorrectPasswordException() { }

    public IncorrectPasswordException(string? message)
        : base(message) { }

    public IncorrectPasswordException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public IncorrectPasswordException(string? message, PdfPassword password)
        : this(message, password, null) { }

    public IncorrectPasswordException(
        string? message,
        PdfPassword password,
        Exception? innerException)
        : base(message ?? GetDefaultMessage(password), innerException)
    {
        Password = password;
    }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    protected IncorrectPasswordException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        Password = (PdfPassword?)info.GetValue(nameof(Password), typeof(PdfPassword))
            ?? default;
    }

    public PdfPassword Password { get; private set; }

#if NET8_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ExtendedArgumentNullException.ThrowIfNull(info, nameof(info));

        info.AddValue(nameof(Password), Password, typeof(PdfPassword));
        base.GetObjectData(info, context);
    }

    private static string GetDefaultMessage(PdfPassword password)
    {
        var passwords = new List<string>();

        if (!string.IsNullOrEmpty(password.UserPassword))
            passwords.Add(password.UserPassword);

        if (!string.IsNullOrEmpty(password.OwnerPassword))
            passwords.Add(password.OwnerPassword);

        return passwords.Count switch
        {
            1 => $"Password {passwords[0]} is incorrect.",
            2 => $"Passwords {string.Join(" and ", passwords)} are incorrect.",
            _ => "Password is incorrect."
        };
    }
}
