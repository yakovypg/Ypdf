using YpdfLib.Models.Paging;

namespace YpdfDesktop.Models.Paging
{
    public interface IPageCroppingInfo
    {
        int PageNumber { get; }
        float PageWidth { get; }
        float PageHeight { get; }
        
        string Presenter { get; }
        bool ExecuteCropping { get; set; }
        
        float UpperRightPointX { get; set; }
        float UpperRightPointY { get; set; }
        float LowerLeftPointX { get; set; }
        float LowerLeftPointY { get; set; }

        PageCropping GetCropping();
    }
}
