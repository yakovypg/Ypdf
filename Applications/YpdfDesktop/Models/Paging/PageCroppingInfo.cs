using System;
using YpdfDesktop.Models.Base;
using YpdfLib.Models.Geometry;
using YpdfLib.Models.Paging;

namespace YpdfDesktop.Models.Paging
{
    public class PageCroppingInfo : ReactiveModel, IPageCroppingInfo
    {
        public int PageNumber { get; }
        public float PageWidth { get; }
        public float PageHeight { get; }

        private bool _executeCropping;
        public bool ExecuteCropping
        {
            get => _executeCropping;
            set => RaiseAndSetIfChanged(ref _executeCropping, value);
        }

        private float _upperRightPointX;
        public float UpperRightPointX
        {
            get => _upperRightPointX;
            set => RaiseAndSetIfChanged(ref _upperRightPointX, value);
        }

        private float _upperRightPointY;
        public float UpperRightPointY
        {
            get => _upperRightPointY;
            set => RaiseAndSetIfChanged(ref _upperRightPointY, value);
        }

        private float _lowerLeftPointX;
        public float LowerLeftPointX
        {
            get => _lowerLeftPointX;
            set => RaiseAndSetIfChanged(ref _lowerLeftPointX, value);
        }

        private float _lowerLeftPointY;
        public float LowerLeftPointY
        {
            get => _lowerLeftPointY;
            set => RaiseAndSetIfChanged(ref _lowerLeftPointY, value);
        }

        public string Presenter => $"[{PageNumber}] {(int)PageWidth}x{(int)PageHeight}";

        public PageCroppingInfo(int pageNumber,
                                float pageWidth,
                                float pageHeight,
                                bool executeCropping = false)
            : this(pageNumber,
                   pageWidth,
                   pageHeight,
                   pageWidth,
                   pageHeight,
                   0,
                   0,
                   executeCropping)
        {
        }

        public PageCroppingInfo(int pageNumber,
                                float pageWidth,
                                float pageHeight,
                                float upprerRightPointX,
                                float upprerRightPointY,
                                float lowerLeftPointX,
                                float lowerLeftPointY,
                                bool executeCropping = false)
        {
            PageNumber = pageNumber;
            PageWidth = pageWidth;
            PageHeight = pageHeight;
            UpperRightPointX = upprerRightPointX;
            UpperRightPointY = upprerRightPointY;
            LowerLeftPointX = lowerLeftPointX;
            LowerLeftPointY = lowerLeftPointY;
            ExecuteCropping = executeCropping;
        }

        public PageCropping GetCropping()
        {
            if (UpperRightPointX < LowerLeftPointX)
                (UpperRightPointX, LowerLeftPointX) = (LowerLeftPointX, UpperRightPointX);
            
            if (UpperRightPointY < LowerLeftPointY)
                (UpperRightPointY, LowerLeftPointY) = (LowerLeftPointY, UpperRightPointY);
            
            var lowerLeft = new FloatPoint(LowerLeftPointX, LowerLeftPointY);
            var upperRight = new FloatPoint(UpperRightPointX, UpperRightPointY);

            return new PageCropping(PageNumber, lowerLeft, upperRight);
        }

        public override string ToString()
        {
            return Presenter;
        }
    }
}
