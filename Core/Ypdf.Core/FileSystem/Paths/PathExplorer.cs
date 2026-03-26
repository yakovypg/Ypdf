using System;
using System.Linq;
using Ypdf.Core.Config;
using Ypdf.Core.Extensions;

namespace Ypdf.Core.FileSystem.Paths;

public static class PathExplorer
{
    public static int GetImageNumberFromPath(
        string path,
        string numberMark = FileMarks.ImageNumber)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(numberMark, nameof(numberMark));

        string errorMessage = $"Cannot get image number from the '{path}'.";
        int numberMarkIndex = path.LastIndexOf(numberMark, StringComparison.Ordinal);

        if (numberMarkIndex < 0 || numberMarkIndex >= path.Length - 1)
            throw new FormatException(errorMessage);

        int tailStartIndex = numberMarkIndex + numberMark.Length;
        string tail = path.Substring(tailStartIndex);

        char delimiter = tail.FirstOrDefault(t => !char.IsDigit(t));
        int delimiterIndex = tail.IndexOf(delimiter, StringComparison.Ordinal);

        if (delimiterIndex < 0 || delimiterIndex >= tail.Length - 1)
            throw new FormatException(errorMessage);

        string imageNumberString = tail.Remove(delimiterIndex);

        return int.TryParse(imageNumberString, out int imageNumber)
            ? imageNumber
            : throw new FormatException(errorMessage);
    }
}
