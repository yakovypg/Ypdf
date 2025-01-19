using System.Collections.Generic;
using System.Linq;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Ypdf.Core.Design;
using Ypdf.Core.Design.Pages;

namespace Ypdf.Core.Tools;

public class ImageToPdfTool : IMultipleInputTool
{
    public ImageToPdfTool()
        : this(new ImagePageParameters()) { }

    public ImageToPdfTool(IImagePageParameters pageParameters)
    {
        ExtendedArgumentNullException.ThrowIfNull(pageParameters, nameof(pageParameters));
        PageParameters = pageParameters;
    }

    protected IImagePageParameters PageParameters { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        Execute(new string[] { inputPath }, outputPath);
    }

    public void Execute(IEnumerable<string> inputPaths, string outputPath)
    {
        ExtendedArgumentNullException.ThrowIfNull(inputPaths, nameof(inputPaths));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfContainsNotExistingFile(inputPaths, nameof(inputPaths));

        Image[] images = inputPaths
            .Select(t => new Image(ImageDataFactory.Create(t)))
            .ToArray();

        using var writer = new PdfWriter(outputPath);
        using var pdfDocument = new PdfDocument(writer);
        using var outputDocument = new Document(pdfDocument);

        ApplyStyleToDocument(outputDocument, images);

        for (int i = 0; i < images.Length; ++i)
        {
            Image image = images[i];

            PageParameters.ApplyStyleToImage(image);
            outputDocument.Add(image);

            if (i < images.Length - 1)
            {
                var areaBreak = new AreaBreak(AreaBreakType.NEXT_PAGE);
                outputDocument.Add(areaBreak);
            }
        }
    }

    private void AddImageDependentPages(PdfDocument document, IEnumerable<Image> images)
    {
        ExtendedArgumentNullException.ThrowIfNull(document, nameof(document));
        ExtendedArgumentNullException.ThrowIfNull(images, nameof(images));

        foreach (Image image in images)
        {
            float horizontalIncrease = PageParameters.AutoIncreaseSize
                ? PageParameters.Margin?.HorizontalSum ?? 0
                : 0;

            float verticalIncrease = PageParameters.AutoIncreaseSize
                ? PageParameters.Margin?.VerticalSum ?? 0
                : 0;

            float imageWidth = image.GetImageWidth() + horizontalIncrease;
            float imageHeight = image.GetImageHeight() + verticalIncrease;

            var pageSize = new PageSize(imageWidth, imageHeight);
            document.AddNewPage(pageSize);
        }
    }

    private void ApplyStyleToDocument(Document document, Image[] images)
    {
        ExtendedArgumentNullException.ThrowIfNull(document, nameof(document));
        ExtendedArgumentNullException.ThrowIfNull(images, nameof(images));

        Margin? margin = PageParameters.Margin;
        PdfDocument pdfDocument = document.GetPdfDocument();

        if (margin is not null)
        {
            document.SetMargins(
                margin.Value.Top,
                margin.Value.Right,
                margin.Value.Bottom,
                margin.Value.Left);
        }

        if (PageParameters.Size is not null)
            pdfDocument.SetDefaultPageSize(PageParameters.Size);
        else
            AddImageDependentPages(pdfDocument, images);
    }
}
