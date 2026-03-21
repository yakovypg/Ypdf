using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ypdf.Core.Security;

[Serializable]
public class IncorrectPasswordException : Exception
{
    public IncorrectPasswordException()
        : this(GetDefaultMessage()) { }

    public IncorrectPasswordException(string? message)
        : base(message ?? GetDefaultMessage()) { }

    public IncorrectPasswordException(string? message, Exception? innerException)
        : base(message ?? GetDefaultMessage(), innerException) { }

    public IncorrectPasswordException(string? message, PdfPassword password)
        : this(message ?? GetDefaultMessage(password), password, null) { }

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

    private static string GetDefaultMessage(PdfPassword? password = null)
    {
        var passwords = new List<string>();

        if (password is not null && !string.IsNullOrEmpty(password.Value.UserPassword))
            passwords.Add(password.Value.UserPassword);

        if (password is not null && !string.IsNullOrEmpty(password.Value.OwnerPassword))
            passwords.Add(password.Value.OwnerPassword);

        return passwords.Count switch
        {
            1 => $"Password {passwords[0]} is incorrect.",
            2 => $"Passwords {string.Join(" and ", passwords)} are incorrect.",
            _ => "Password is incorrect."
        };
    }
}
