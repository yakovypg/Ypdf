using System.Collections.Generic;
using System.Linq;
using iText.Kernel.Pdf;

namespace Ypdf.Core.Extensions;

internal static class PdfPageExtensions
{
    internal static IList<PdfName> GetImageReferences(this PdfPage page)
    {
        ExtendedArgumentNullException.ThrowIfNull(page, nameof(page));
        return GetImageReferences(page, out _);
    }

    internal static IList<PdfName> GetImageReferences(
        this PdfPage page,
        out PdfDictionary? resourcesXObjects)
    {
        ExtendedArgumentNullException.ThrowIfNull(page, nameof(page));

        PdfDictionary? pdfObject = page.GetPdfObject();
        PdfDictionary? resources = pdfObject?.GetAsDictionary(PdfName.Resources);

        resourcesXObjects = resources?.GetAsDictionary(PdfName.XObject);

        return resourcesXObjects?.KeySet().ToList() ?? [];
    }
}
