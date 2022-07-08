namespace YpdfLib.Models.Design
{
    public class Margin : IMargin
    {
        public float Top { get; }
        public float Right { get; }
        public float Bottom { get; }
        public float Left { get; }

        public float HorizontalSum => Left + Right;
        public float VerticalSum => Top + Bottom;

        public Margin() : this(0)
        {
        }

        public Margin(float value) : this(value, value, value, value)
        {
        }

        public Margin(float horizontal, float vertical) : this(horizontal, vertical, horizontal, vertical)
        {
        }

        public Margin(float left, float top, float right, float bottom)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }
    }
}
