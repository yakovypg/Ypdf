using System;
using System.Collections.Generic;
using iText.Kernel.Geom;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public class PageParameters : IPageParameters, IEquatable<PageParameters?>
{
    public PageParameters(Margin? margin = null, PageSize? pageSize = null)
    {
        Margin = margin;
        Size = pageSize;
    }

    public virtual Margin? Margin { get; init; }
    public virtual PageSize? Size { get; init; }

    public bool Equals(PageParameters? other)
    {
        return other is not null
            && EqualityComparer<Margin?>.Default.Equals(Margin, other.Margin)
            && EqualityComparer<PageSize?>.Default.Equals(Size, other.Size);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PageParameters);
    }

    public override int GetHashCode()
    {
        return HashGenerator.Generate(Margin, Size);
    }
}
