namespace FileSystemLib.Paths
{
    public static class PathsSplitter
    {
        public static IEnumerable<string[]> Split(IEnumerable<string> paths, int itemsInParts)
        {
            IEnumerable<string> remainingItems = paths;

            while (remainingItems.Any())
            {
                var currentItems = remainingItems.Take(itemsInParts);
                remainingItems = remainingItems.Skip(itemsInParts);

                yield return currentItems.ToArray();
            }
        }
    }
}
