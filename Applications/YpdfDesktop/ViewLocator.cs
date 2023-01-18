using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using YpdfDesktop.ViewModels;

namespace YpdfDesktop
{
    public class ViewLocator : IDataTemplate
    {
        public IControl Build(object data)
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            return type is not null
                ? (Control)Activator.CreateInstance(type)!
                : throw new NotImplementedException("View");
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}
