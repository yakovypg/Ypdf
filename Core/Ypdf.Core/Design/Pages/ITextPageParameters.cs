using iText.Layout.Properties;
using Ypdf.Core.Design.Fonts;

namespace Ypdf.Core.Design.Pages;

public interface ITextPageParameters : IPageParameters
{
    TextFontInfo FontInfo { get; }
    TextAlignment HorizontalAlignment { get; }
}
