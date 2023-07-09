using YpdfLib.Models.Paging;

namespace YpdfDesktop.Models.Paging
{
    public interface IPageDivisionInfo
    {
        int PageNumber { get; }
        float PageWidth { get; }
        float PageHeight { get; }
        
        string Presenter { get; }
        bool ExecuteDivision { get; set; }
        
        PageDivisionOrientation Orientation { get; set; }
        int HorizontalDivisionPoint { get; set; }
        int VerticalDivisionPoint { get; set; }

        PageDivision GetDivision();
    }
}
