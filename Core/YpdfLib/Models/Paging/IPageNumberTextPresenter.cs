namespace YpdfLib.Models.Paging
{
    public interface IPageNumberTextPresenter : IDeepCloneable<IPageNumberTextPresenter>
    {
        Func<int, int, string> Converter { get; }

        string GetText(int pageNum, int numOfPages);
    }
}
