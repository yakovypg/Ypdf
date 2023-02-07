using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Reactive.Linq;

namespace YpdfDesktop.Infrastructure.Behaviors.Text
{
    public abstract class NumberBoxBehavior : TextValidatorBehavior<TextBox>
    {
        protected override void OnAttachedToVisualTree()
        {
            base.OnAttachedToVisualTree();

            if (AssociatedObject is null)
                return;

            ApplyEvent(AssociatedObject, InputElement.TextInputEvent, RoutingStrategies.Tunnel)
                .Do(t => CheckCorrectness(t.EventArgs.Text, AssociatedObject, t.EventArgs))
                .Subscribe();

            ApplyEvent(AssociatedObject, TextBox.PastingFromClipboardEvent, RoutingStrategies.Bubble)
                .Do(async t => CheckCorrectness(await GetClipboardText(), AssociatedObject, t.EventArgs))
                .Subscribe();
        }

        protected virtual void CheckCorrectness(string? receivedText, TextBox textBox, RoutedEventArgs e)
        {
            string possibleText = GetPossibleText(receivedText, textBox);
            e.Handled = !IsPossibleTextValid(possibleText, textBox.Text);
        }

        protected static string GetPossibleText(string? receivedText, TextBox textBox)
        {
            int selectionStart = Math.Min(textBox.SelectionStart, textBox.SelectionEnd);
            int selectionEnd = Math.Max(textBox.SelectionStart, textBox.SelectionEnd);

            string textBeforeSelection = textBox.Text[..selectionStart];
            string textAfterSelection = textBox.Text[selectionEnd..];

            return $"{textBeforeSelection}{receivedText}{textAfterSelection}";
        }
    }
}
