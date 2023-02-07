using System.Globalization;

namespace YpdfDesktop.Infrastructure.Behaviors.Text
{
    public class PositiveFloatNumberBoxBehavior : FloatNumberBoxBehavior
    {
        protected override bool IsPossibleTextValid(string possibleText, string currentText)
        {
            string dot = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            bool isStartZero = possibleText == "0" || possibleText == $"0{dot}";

            if (isStartZero)
                return true;

            NumberStyles numStyle = NumberStyles.AllowDecimalPoint;

            return float.TryParse(possibleText, numStyle, CultureInfo.CurrentUICulture, out float value)
                && !float.IsNaN(value)
                && !float.IsInfinity(value)
                && value > 0;
        }
    }
}
