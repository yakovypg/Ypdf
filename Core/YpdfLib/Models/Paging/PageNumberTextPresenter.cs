namespace YpdfLib.Models.Paging
{
    public class PageNumberTextPresenter : IPageNumberTextPresenter
    {
        public static readonly PageNumberTextPresenter DefaultPresenter = new ((pageNum, numOfPages) => pageNum.ToString());
        public static readonly PageNumberTextPresenter FractionalPresenter = new((pageNum, numOfPages) => $"{pageNum}/{numOfPages}");
        public static readonly PageNumberTextPresenter VerbalPresenter = new((pageNum, numOfPages) => $"page {pageNum} of {numOfPages}");

        private readonly Func<int, int, string> _converter;

        public PageNumberTextPresenter(Func<int, int, string> converter)
        {
            _converter = converter;
        }

        public string GetText(int pageNum, int numOfPages)
        {
            return _converter.Invoke(pageNum, numOfPages);
        }
    }
}
