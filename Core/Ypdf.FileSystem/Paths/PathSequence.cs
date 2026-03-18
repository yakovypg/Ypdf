using System;
using System.Collections.Generic;
using System.Linq;

namespace Ypdf.FileSystem.Paths;

public class PathSequence
{
    public PathSequence(IEnumerable<string> paths)
    {
        Paths = paths ?? throw new ArgumentNullException(nameof(paths));
    }

    public IEnumerable<string> Paths { get; }

    public IEnumerable<IEnumerable<string>> Group(int itemsInGroup = 5)
    {
        if (itemsInGroup <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(itemsInGroup),
                itemsInGroup,
                $"{itemsInGroup} must be greater than zero.");
        }

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
