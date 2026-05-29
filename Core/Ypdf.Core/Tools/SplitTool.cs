using System;
using System.Collections.Generic;
using System.Linq;
using iText.Kernel.Utils;
using Ypdf.Core.Geometry;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Tools;

public class SplitTool : ITool, IMultipleOutputTool
{
    public SplitTool(long splitPartSizeBytes = Splitter.DefaultSplitPartSizeBytes)
        : this(splitPartSizeBytes, null) { }

    public SplitTool(MathExpression splitPartSizeBytesExpression)
        : this(
            splitPartSizeBytesExpression?.CalculateLong()
                ?? throw new ArgumentNullException(nameof(splitPartSizeBytesExpression)),
            null)
    { }

    public SplitTool(IEnumerable<string> pageRanges)
        : this(pageRanges.Select(t => new PageRange(t))) { }

    public SplitTool(IEnumerable<Enumeration.PageRange> pageRanges)
        : this(pageRanges.Select(t => t.ToKernelPageRange())) { }

    public SplitTool(IEnumerable<PageRange> pageRanges)
        : this(
            Splitter.DefaultSplitPartSizeBytes,
            pageRanges ?? throw new ArgumentNullException(nameof(pageRanges)))
    { }

    protected SplitTool(
        long splitPartSizeBytes = Splitter.DefaultSplitPartSizeBytes,
        IEnumerable<PageRange>? pageRanges = null)
    {
        DefaultExceptions.ThrowIfNegativeOrZero(splitPartSizeBytes, nameof(splitPartSizeBytes));

        SplitPartSizeBytes = splitPartSizeBytes;
        PageRanges = pageRanges ?? [];
    }

    protected long SplitPartSizeBytes { get; }
    protected IEnumerable<PageRange> PageRanges { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfDirectoryNotExists(outputPath, nameof(outputPath));

        using var splitter = new Splitter(inputPath, outputPath);

        if (PageRanges.Any())
            splitter.Split(PageRanges);
        else
            splitter.Split(SplitPartSizeBytes);
    }
}
