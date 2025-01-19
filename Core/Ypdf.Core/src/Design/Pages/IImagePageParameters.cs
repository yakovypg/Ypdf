using iText.Layout.Element;
using iText.Layout.Properties;

namespace Ypdf.Core.Design.Pages;

public interface IImagePageParameters : IPageParameters
{
    double RotationAngle { get; }
    bool AutoIncreaseSize { get; }
    HorizontalAlignment HorizontalAlignment { get; }

    void ApplyStyleToImage(Image image);
}
