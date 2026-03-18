using System;
using iText.Kernel.Pdf;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Security;

public readonly struct PdfPassword : IEquatable<PdfPassword>
{
    public PdfPassword(
        string userPassword,
        string ownerPassword = "",
        int encryptionAlgorithm = EncryptionConstants.ENCRYPTION_AES_128)
    {
        ExtendedArgumentNullException.ThrowIfNull(userPassword, nameof(userPassword));
        ExtendedArgumentNullException.ThrowIfNull(ownerPassword, nameof(ownerPassword));

        if (string.IsNullOrEmpty(userPassword) && string.IsNullOrEmpty(ownerPassword))
            throw new NoPasswordSpecifiedException();

        UserPassword = userPassword;
        OwnerPassword = ownerPassword;
        EncryptionAlgorithm = encryptionAlgorithm;
    }

    public readonly string UserPassword { get; }
    public readonly string OwnerPassword { get; }
    public readonly int EncryptionAlgorithm { get; }

    public readonly string MainPassword => string.IsNullOrEmpty(OwnerPassword)
        ? UserPassword
        : OwnerPassword;

    public static bool operator ==(PdfPassword left, PdfPassword right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PdfPassword left, PdfPassword right)
    {
        return !(left == right);
    }

    public readonly bool Equals(PdfPassword other)
    {
        return UserPassword == other.UserPassword
            && OwnerPassword == other.OwnerPassword
            && EncryptionAlgorithm == other.EncryptionAlgorithm;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PdfPassword other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(
            UserPassword,
            OwnerPassword,
            EncryptionAlgorithm);
    }
}
