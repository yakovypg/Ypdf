using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace YpdfDesktop.Infrastructure.Behaviors.Text
{
    public abstract class TextBehavior<T> : Behavior<T> where T : class, IAvaloniaObject
    {
        protected Task<string> GetClipboardText()
        {
            return Application.Current?.Clipboard?.GetTextAsync()
                ?? new Task<string>(() => string.Empty);
        }

        protected static IObservable<EventPattern<E>> ApplyEvent<E>(IInteractive target, RoutedEvent<E> e,
            RoutingStrategies strategies) where E : RoutedEventArgs
        {
            return Observable.FromEventPattern<E>(
                addHandler => target.AddHandler(e, addHandler, strategies),
                removeHandler => target.RemoveHandler(e, removeHandler));
        }
    }
}
