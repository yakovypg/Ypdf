using System;
using YpdfDesktop.Models.Base;
using YpdfLib.Models.Paging;

namespace YpdfDesktop.Models.Paging
{
    public class PageDivisionInfo : ReactiveModel, IPageDivisionInfo
    {
        public int PageNumber { get; }
        public float PageWidth { get; }
        public float PageHeight { get; }

        private bool _executeDivision;
        public bool ExecuteDivision
        {
            get => _executeDivision;
            set => RaiseAndSetIfChanged(ref _executeDivision, value);
        }

        private PageDivisionOrientation _orientation;
        public PageDivisionOrientation Orientation
        {
            get => _orientation;
            set => RaiseAndSetIfChanged(ref _orientation, value);
        }

        private int _horizontalDivisionPoint;
        public int HorizontalDivisionPoint
        {
            get => _horizontalDivisionPoint;
            set => RaiseAndSetIfChanged(ref _horizontalDivisionPoint, value);
        }

        private int _verticalDivisionPoint;
        public int VerticalDivisionPoint
        {
            get => _verticalDivisionPoint;
            set => RaiseAndSetIfChanged(ref _verticalDivisionPoint, value);
        }

        public string Presenter => $"[{PageNumber}] {(int)PageWidth}x{(int)PageHeight}";

        public PageDivisionInfo(int pageNumber,
                                float pageWidth,
                                float pageHeight,
                                PageDivisionOrientation orientation = PageDivisionOrientation.Vertical,
                                bool executeDivision = false)
            : this(pageNumber, pageWidth, pageHeight, orientation, (int)(pageHeight / 2), (int)(pageWidth / 2), executeDivision)
        {
        }

        public PageDivisionInfo(int pageNumber,
                                float pageWidth,
                                float pageHeight,
                                PageDivisionOrientation orientation,
                                int horizontalDivisionPoint,
                                int verticalDivisionPoint,
                                bool executeDivision = false)
        {
            PageNumber = pageNumber;
            PageWidth = pageWidth;
            PageHeight = pageHeight;
            Orientation = orientation;
            HorizontalDivisionPoint = horizontalDivisionPoint;
            VerticalDivisionPoint = verticalDivisionPoint;
            ExecuteDivision = executeDivision;
        }

        public PageDivision GetDivision()
        {
            float pageHorizontalCenter = PageWidth / 2;
            float pageVerticalCenter = PageHeight / 2;

            float centerOffset = Orientation switch
            {
                PageDivisionOrientation.Vertical => VerticalDivisionPoint - pageVerticalCenter,
                PageDivisionOrientation.Horizontal => HorizontalDivisionPoint - pageHorizontalCenter,
                _ => throw new NotImplementedException()
            };

            return new PageDivision(PageNumber, Orientation, centerOffset);
        }

        public override string ToString()
        {
            return Presenter;
        }
    }
}