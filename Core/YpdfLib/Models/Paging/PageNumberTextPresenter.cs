namespace YpdfLib.Models.Paging
{
    public class PageNumberTextPresenter : IPageNumberTextPresenter, IDeepCloneable<PageNumberTextPresenter>, IEquatable<PageNumberTextPresenter?>
    {
        public static readonly PageNumberTextPresenter DefaultPresenter = new((pageNum, numOfPages) => pageNum.ToString());
        public static readonly PageNumberTextPresenter FractionalPresenter = new((pageNum, numOfPages) => $"{pageNum}/{numOfPages}");
        public static readonly PageNumberTextPresenter VerbalPresenter = new((pageNum, numOfPages) => $"page {pageNum} of {numOfPages}");

        public Func<int, int, string> Converter { get; }

        public PageNumberTextPresenter(Func<int, int, string>? converter = null)
        {
            Converter = converter ?? DefaultPresenter.Converter;
        }

        public string GetText(int pageNum, int numOfPages)
        {
            return Converter.Invoke(pageNum, numOfPages);
        }

        public PageNumberTextPresenter Copy()
        {
            return new PageNumberTextPresenter(Converter);
        }

        IPageNumberTextPresenter IDeepCloneable<IPageNumberTextPresenter>.Copy()
        {
            return Copy();
        }

        public bool Equals(PageNumberTextPresenter? other)
        {
            return other is not null &&
                   EqualityComparer<Func<int, int, string>>.Default.Equals(Converter, other.Converter);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PageNumberTextPresenter);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Converter);
        }
    }
}
