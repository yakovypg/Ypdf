using iText.Kernel.Geom;

namespace Ypdf.Core.Design.Pages;

public interface IPageParameters
{
    Margin? Margin { get; }
    PageSize? Size { get; }
}
