namespace YpdfDesktop.Models.Enumeration
{
    public interface IRange
    {
        int Min { get; }
        int Max { get; }

        Range Self { get; }

        int Start { get; set; }
        int End { get; set; }

        bool IsStartCorrect { get; }
        bool IsEndCorrect { get; }
        bool IsCorrect { get; }
    }
}