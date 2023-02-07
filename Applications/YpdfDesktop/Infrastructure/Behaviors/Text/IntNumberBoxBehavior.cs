using System.Globalization;

namespace YpdfDesktop.Infrastructure.Behaviors.Text
{
    public class IntNumberBoxBehavior : NumberBoxBehavior
    {
        protected override bool IsPossibleTextValid(string possibleText, string currentText)
        {
            string negativeSign = CultureInfo.CurrentUICulture.NumberFormat.NegativeSign;
            bool isCorrectSign = possibleText == negativeSign && currentText == string.Empty;

            return isCorrectSign || int.TryParse(possibleText, out _);
        }
    }
}
