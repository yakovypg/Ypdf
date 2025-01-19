using System.Collections.Generic;
using System.Linq;
using iText.Kernel.Utils;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Tools;

public class SplitTool : ITool
{
    public SplitTool(IEnumerable<string> pageRanges)
        : this(pageRanges.Select(t => new PageRange(t))) { }

    public SplitTool(IEnumerable<PageRange> pageRanges)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(pageRanges, nameof(pageRanges));
        PageRanges = pageRanges;
    }

    protected IEnumerable<PageRange> PageRanges { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfDirectoryNotExists(outputPath, nameof(outputPath));

        using var splitter = new Splitter(inputPath, outputPath);
        splitter.Split(PageRanges);
    }
}
