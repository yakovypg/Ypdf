using System;
using System.Collections.Generic;
using System.Globalization;
using iText.Kernel.Pdf;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Security;

public readonly struct EncryptionAlgorithm : IEquatable<EncryptionAlgorithm>
{
    public EncryptionAlgorithm()
        : this(EncryptionConstants.ENCRYPTION_AES_128)
    {
    }

    public EncryptionAlgorithm(int encryptionConstant)
    {
        DefaultExceptions.ThrowIfNotAllowed(
            encryptionConstant,
            SupportedEncryptionConstants,
            nameof(encryptionConstant));

        EncryptionConstant = encryptionConstant;
    }

    public static IReadOnlyCollection<int> SupportedEncryptionConstants =>
    [
        EncryptionConstants.STANDARD_ENCRYPTION_40,
        EncryptionConstants.STANDARD_ENCRYPTION_128,
        EncryptionConstants.ENCRYPTION_AES_128,
        EncryptionConstants.ENCRYPTION_AES_256
    ];

    public readonly int EncryptionConstant { get; }

    public static bool operator ==(EncryptionAlgorithm left, EncryptionAlgorithm right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EncryptionAlgorithm left, EncryptionAlgorithm right)
    {
        return !(left == right);
    }

    public static EncryptionAlgorithm Parse(string data)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(data, nameof(data));

        return data.ToUpper(CultureInfo.CurrentCulture) switch
        {
            nameof(EncryptionConstants.STANDARD_ENCRYPTION_40) =>
                new EncryptionAlgorithm(EncryptionConstants.STANDARD_ENCRYPTION_40),

            nameof(EncryptionConstants.STANDARD_ENCRYPTION_128) =>
                new EncryptionAlgorithm(EncryptionConstants.STANDARD_ENCRYPTION_128),

            nameof(EncryptionConstants.ENCRYPTION_AES_128) =>
                new EncryptionAlgorithm(EncryptionConstants.ENCRYPTION_AES_128),

            nameof(EncryptionConstants.ENCRYPTION_AES_256) =>
                new EncryptionAlgorithm(EncryptionConstants.ENCRYPTION_AES_256),

            _ => throw new ArgumentOutOfRangeException(nameof(data), data)
        };
    }

    public readonly bool Equals(EncryptionAlgorithm other)
    {
        return EncryptionConstant == other.EncryptionConstant;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is EncryptionAlgorithm other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(EncryptionConstant);
    }
}
