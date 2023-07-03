namespace YpdfDesktop.Infrastructure.Behaviors.Text
{
    public class NotNegativeIntNumberBoxBehavior : IntNumberBoxBehavior
    {
        protected override bool IsPossibleTextValid(string? possibleText, string? currentText)
        {
            possibleText ??= string.Empty;       
            return int.TryParse(possibleText, out int value) && value >= 0;
        }
    }
}