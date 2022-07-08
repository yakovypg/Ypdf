namespace YpdfLib.Models.Paging
{
    public interface IPageNumberTextPresenter
    {
        string GetText(int pageNum, int numOfPages);
    }
}
