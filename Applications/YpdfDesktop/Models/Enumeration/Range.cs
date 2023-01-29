using YpdfDesktop.Models.Base;

namespace YpdfDesktop.Models.Enumeration
{
    public class Range : ReactiveModel, IRange
    {
        public int Min { get; }
        public int Max { get; }

        public Range Self => this;

        private int _start = 1;
        public int Start
        {
            get => _start;
            set
            {
                if (RaiseAndSetIfChanged(ref _start, value))
                    VerifyEnds();
            }
        }

        private int _end = 1;
        public int End
        {
            get => _end;
            set
            {
                if (RaiseAndSetIfChanged(ref _end, value))
                    VerifyEnds();
            }
        }

        private bool _isStartCorrect = true;
        public bool IsStartCorrect
        {
            get => _isStartCorrect;
            private set => RaiseAndSetIfChanged(ref _isStartCorrect, value);
        }

        private bool _isEndCorrect = true;
        public bool IsEndCorrect
        {
            get => _isEndCorrect;
            private set => RaiseAndSetIfChanged(ref _isEndCorrect, value);
        }

        private bool _isCorrect = true;
        public bool IsCorrect
        {
            get => _isCorrect;
            private set => RaiseAndSetIfChanged(ref _isCorrect, value);
        }

        public Range(int min, int max) : this(min, max, min, max)
        {
        }

        public Range(int min, int max, int start, int end)
        {
            Min = min;
            Max = max;
            Start = start;
            End = end;
        }

        private void VerifyEnds()
        {
            IsStartCorrect = _start >= Min;
            IsEndCorrect = _end <= Max && _start <= _end;
            IsCorrect = IsStartCorrect && IsEndCorrect;
        }
    }
}
