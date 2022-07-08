namespace YpdfLib.Models.Paging
{
    public class PageRotation : IPageRotation
    {
        public int PageNumber { get; }
        public int Angle { get; }

        public PageRotation(int pageNumber, int angle)
        {
            PageNumber = pageNumber;
            Angle = angle;
        }
    }
}
