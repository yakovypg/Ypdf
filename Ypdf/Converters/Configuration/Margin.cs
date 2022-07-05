namespace Ypdf.Converters.Configuration
{
    public class Margin
    {
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }

        public float HorizontalSum => Left + Right;
        public float VerticalSum => Top + Bottom;

        public Margin() : this(0)
        {
        }

        public Margin(float value) : this(value, value, value, value)
        {
        }

        public Margin(float horizontal, float vertical) : this(vertical, horizontal, vertical, horizontal)
        {
        }

        public Margin(float top, float right, float bottom, float left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }
    }
}
