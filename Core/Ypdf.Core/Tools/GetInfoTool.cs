using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ypdf.Core.Extensions;
using Ypdf.Core.Informing;
using Ypdf.Core.Runtime.Logging;

namespace Ypdf.Core.Tools;

public class GetInfoTool : ITool
{
    public GetInfoTool(int maxPageSizesToPrint = 0, IOutputWriter? outputWriter = null)
    {
        MaxPageSizesToPrint = maxPageSizesToPrint;
        OutputWriter = outputWriter;
    }

    protected int MaxPageSizesToPrint { get; }
    protected IOutputWriter? OutputWriter { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        PdfInfo pdfInfo = PdfInfo.Collect(inputPath);

        if (!string.IsNullOrWhiteSpace(outputPath))
        {
            using var fileWriter = new StreamWriter(outputPath);
            PrintInfo(pdfInfo, fileWriter.WriteLine);
        }
        else if (OutputWriter is not null)
        {
            PrintInfo(pdfInfo, OutputWriter.WriteLine);
        }
    }

    protected virtual void PrintInfo(
        PdfInfo pdfInfo,
        Action<string?> printLine)
    {
        ExtendedArgumentNullException.ThrowIfNull(pdfInfo, nameof(pdfInfo));
        ExtendedArgumentNullException.ThrowIfNull(printLine, nameof(printLine));

        PrintBasicInfo(pdfInfo, printLine);
        PrintFileSize(pdfInfo, printLine);
        PrintPageSizes(pdfInfo, printLine);
    }

    protected virtual void PrintBasicInfo(
        PdfInfo pdfInfo,
        Action<string?> printLine)
    {
        ExtendedArgumentNullException.ThrowIfNull(pdfInfo, nameof(pdfInfo));
        ExtendedArgumentNullException.ThrowIfNull(printLine, nameof(printLine));

        printLine.Invoke($"Name: {pdfInfo.Name}");
        printLine.Invoke($"Pages: {pdfInfo.NumberOfPages}");
        printLine.Invoke($"Creation time: {pdfInfo.CreationTime}");
        printLine.Invoke($"Last access time: {pdfInfo.LastAccessTime}");
        printLine.Invoke($"Last write time: {pdfInfo.LastWriteTime}");
    }

    protected virtual void PrintFileSize(
        PdfInfo pdfInfo,
        Action<string?> printLine)
    {
        ExtendedArgumentNullException.ThrowIfNull(pdfInfo, nameof(pdfInfo));
        ExtendedArgumentNullException.ThrowIfNull(printLine, nameof(printLine));

        double sizeMb = pdfInfo.SizeBytes / 1024.0 / 1024.0;
        double sizeMbRounded = Math.Round(sizeMb, 2);

        printLine.Invoke($"Size (MB): {sizeMbRounded}");
    }

    protected virtual void PrintPageSizes(
        PdfInfo pdfInfo,
        Action<string?> printLine)
    {
        ExtendedArgumentNullException.ThrowIfNull(pdfInfo, nameof(pdfInfo));
        ExtendedArgumentNullException.ThrowIfNull(printLine, nameof(printLine));

        printLine.Invoke("Page sizes:");

        IEnumerable<string> pageSizePresenters = pdfInfo.PageSizes
            .Select(t => t.ToString(true));

        if (MaxPageSizesToPrint > 0)
            pageSizePresenters = pageSizePresenters.Take(MaxPageSizesToPrint);

        int pageNumber = 1;

        foreach (string pageSizePresenter in pageSizePresenters)
        {
            printLine.Invoke($"{pageNumber++}. {pageSizePresenter}");
        }
    }
}
