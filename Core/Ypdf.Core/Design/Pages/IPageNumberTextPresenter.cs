using System;

namespace Ypdf.Core.Design.Pages;

public interface IPageNumberTextPresenter
{
    Func<int, int, string> Converter { get; }
    string GetText(int pageNum, int numOfPages);
}
