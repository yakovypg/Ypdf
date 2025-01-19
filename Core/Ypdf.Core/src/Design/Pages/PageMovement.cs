using System;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using Ypdf.Core.Utils;

namespace Ypdf.Core.Design.Pages;

public readonly struct PageMovement : IEquatable<PageMovement>
{
    public PageMovement(PdfPage page, int position)
    {
        ExtendedArgumentNullException.ThrowIfNull(page, nameof(page));
        DefaultExceptions.ThrowIfLessThan(position, 1, nameof(position));

        Page = page;
        Position = position;
    }

    public readonly PdfPage Page { get; }
    public readonly int Position { get; }

    public static bool operator ==(PageMovement left, PageMovement right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PageMovement left, PageMovement right)
    {
        return !(left == right);
    }

    public readonly bool Equals(PageMovement other)
    {
        return EqualityComparer<PdfPage>.Default.Equals(Page, other.Page)
            && Position == other.Position;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is PageMovement other
            && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashGenerator.Generate(Page, Position);
    }
}
