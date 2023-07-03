using System;
using System.Reflection;
using YpdfDesktop.Models.Localization;

namespace YpdfDesktop.Infrastructure.Reflection
{
    public static class LazyLocalizer
    {
        public static void LazyLocalize(object sourceObject)
        {
            PropertyInfo[] props = sourceObject.GetType().GetProperties();

            foreach (var p in props)
            {
                Type? interfaceType = p.PropertyType.GetInterface(nameof(ILazyLocalizable));

                if (interfaceType is not null)
                {
                    var localizable = p.GetValue(sourceObject) as ILazyLocalizable;
                    localizable?.Localize();
                }
            }
        }
    }
}