using System.Globalization;

namespace YpdfDesktop.Infrastructure.Behaviors.Text
{
    public class CoefficientNumberBoxBehavior : PositiveFloatNumberBoxBehavior
    {
        protected override bool IsPossibleTextValid(string? possibleText, string? currentText)
        {
            possibleText ??= string.Empty;        
            NumberStyles numStyle = NumberStyles.AllowDecimalPoint;

            return float.TryParse(possibleText, numStyle, CultureInfo.CurrentUICulture, out float value)
                && !float.IsNaN(value)
                && !float.IsInfinity(value)
                && value >= 0
                && value <= 1;
        }
    }
}
