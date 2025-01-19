using System;

namespace Ypdf.Core.Utils;

internal static class HashGenerator
{
    internal const int DefaultSeed = 1009;
    internal const int DefaultFactor = 9176;

    internal static int Generate(int seed, int factor, params object?[] values)
    {
        ExtendedArgumentNullException.ThrowIfNull(values, nameof(values));

        int hash = seed;

        unchecked
        {
            foreach (object? obj in values)
            {
                int objHash = obj?.GetHashCode() ?? 0;
                hash = (hash * factor) + objHash;
            }
        }

        return hash;
    }

    internal static int Generate(params object?[] values)
    {
        ExtendedArgumentNullException.ThrowIfNull(values, nameof(values));

#if NET5_0_OR_GREATER
        HashCode hash = default;

        foreach (object? obj in values)
        {
            hash.Add(obj);
        }

        return hash.ToHashCode();
#else
        return Generate(DefaultSeed, DefaultFactor, values);
#endif
    }
}
