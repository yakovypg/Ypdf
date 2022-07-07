namespace Ypdf.Converters.Configuration
{
    public struct PageRotation
    {
        public int PageNumber { get; set; }
        public int Angle { get; set; }

        public PageRotation(int pageNumber, int angle)
        {
            PageNumber = pageNumber;
            Angle = angle;
        }
    }
}
