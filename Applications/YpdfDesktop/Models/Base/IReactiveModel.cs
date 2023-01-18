using System.Runtime.CompilerServices;

namespace YpdfDesktop.Models.Base
{
    public interface IReactiveModel
    {
        bool RaiseAndSetIfChanged<T>(ref T backingField, T newValue, [CallerMemberName] string? propertyName = null);
    }
}