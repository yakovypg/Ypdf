using System.Globalization;

namespace YpdfDesktop.Infrastructure.Behaviors.Text
{
    public class FloatNumberBoxBehavior : NumberBoxBehavior
    {
        protected override bool IsPossibleTextValid(string possibleText, string currentText)
        {
            string negativeSign = CultureInfo.CurrentUICulture.NumberFormat.NegativeSign;
            bool isCorrectSign = possibleText == negativeSign && currentText == string.Empty;

            if (isCorrectSign)
                return true;

            NumberStyles numStyle = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;

            return float.TryParse(possibleText, numStyle, CultureInfo.CurrentUICulture, out float value)
                && !float.IsNaN(value)
                && !float.IsInfinity(value);
        }
    }
}
