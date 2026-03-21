using System;
using System.Collections.Generic;
using System.Linq;

namespace Ypdf.Core.FileSystem.Paths;

public class PathSequence
{
    public PathSequence(IEnumerable<string> paths)
    {
        ExtendedArgumentNullException.ThrowIfNull(paths, nameof(paths));
        Paths = paths;
    }

    public IEnumerable<string> Paths { get; }

    public IEnumerable<IEnumerable<string>> Group(int itemsInGroup = 5)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(itemsInGroup, nameof(itemsInGroup));

        IReadOnlyList<string> remainingItems = [.. Paths];

        while (remainingItems.Any())
        {
            IEnumerable<string> currentItems = remainingItems.Take(itemsInGroup);

            remainingItems = remainingItems
                .Skip(itemsInGroup)
                .ToList();

            yield return currentItems.ToArray();
        }
    }
}
