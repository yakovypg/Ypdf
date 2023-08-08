namespace YpdfLib.Models.Geometry
{
    public class NotRectangleException : ApplicationException
    {
        private const string DEFAULT_MESSAGE = "Specified points do not represent a rectangle.";
        private readonly FloatPoint[] _points;
        
        public NotRectangleException(FloatPoint[] points, string? message = null, Exception? innerException = null)
            : base(message ?? DEFAULT_MESSAGE, innerException)
        {
            ArgumentNullException.ThrowIfNull(points, nameof(points));
            
            _points = new FloatPoint[points.Length];
            points.CopyTo(points, 0);
        }

        public IReadOnlyCollection<FloatPoint> Points => _points;
    }
}
