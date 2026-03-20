using System;
using System.Collections.Generic;
using System.Linq;
using iText.Kernel.Geom;

namespace Ypdf.Core.Extensions;

public static class PageSizeExtensions
{
    public static bool IsCommonPageSize(
        this Rectangle pageSize,
        out PageSize? commonPageSize,
        out string? commonPageSizeName,
        float comparisonEpsilon = 1e-3f)
    {
        ExtendedArgumentNullException.ThrowIfNull(pageSize, nameof(pageSize));

        var commonPageSizes = new Dictionary<string, PageSize>()
        {
            { nameof(PageSize.A0), PageSize.A0 },
            { nameof(PageSize.A1), PageSize.A1 },
            { nameof(PageSize.A2), PageSize.A2 },
            { nameof(PageSize.A3), PageSize.A3 },
            { nameof(PageSize.A4), PageSize.A4 },
            { nameof(PageSize.A5), PageSize.A5 },
            { nameof(PageSize.A6), PageSize.A6 },
            { nameof(PageSize.A7), PageSize.A7 },
            { nameof(PageSize.A8), PageSize.A8 },
            { nameof(PageSize.A9), PageSize.A9 },
            { nameof(PageSize.A10), PageSize.A10 },
            { nameof(PageSize.B0), PageSize.B0 },
            { nameof(PageSize.B1), PageSize.B1 },
            { nameof(PageSize.B2), PageSize.B2 },
            { nameof(PageSize.B3), PageSize.B3 },
            { nameof(PageSize.B4), PageSize.B4 },
            { nameof(PageSize.B5), PageSize.B5 },
            { nameof(PageSize.B6), PageSize.B6 },
            { nameof(PageSize.B7), PageSize.B7 },
            { nameof(PageSize.B8), PageSize.B8 },
            { nameof(PageSize.B9), PageSize.B9 },
            { nameof(PageSize.B10), PageSize.B10 },
            { nameof(PageSize.EXECUTIVE), PageSize.EXECUTIVE },
            { nameof(PageSize.LEDGER), PageSize.LEDGER },
            { nameof(PageSize.LEGAL), PageSize.LEGAL },
            { nameof(PageSize.LETTER), PageSize.LETTER },
            { nameof(PageSize.TABLOID), PageSize.TABLOID }
        };

        KeyValuePair<string, PageSize> foundCommonPageSizeInfo = commonPageSizes.FirstOrDefault(
            t => t.Value.EqualsWithEpsilon(pageSize, comparisonEpsilon));

        if (string.IsNullOrEmpty(foundCommonPageSizeInfo.Key))
        {
            commonPageSize = null;
            commonPageSizeName = null;
            return false;
        }

        commonPageSize = foundCommonPageSizeInfo.Value;
        commonPageSizeName = foundCommonPageSizeInfo.Key;

        return true;
    }

    public static string ToString(this Rectangle pageSize, bool addCommonPageSizePostfix)
    {
        ExtendedArgumentNullException.ThrowIfNull(pageSize, nameof(pageSize));

        double width = Math.Round(pageSize.GetWidth(), 2);
        double height = Math.Round(pageSize.GetHeight(), 2);

        string presenter = $"{width}x{height}";
        string? commonPageSizeName = null;

        bool canAddCommonPageSizePostfix =
            addCommonPageSizePostfix &&
            IsCommonPageSize(pageSize, out _, out commonPageSizeName) &&
            !string.IsNullOrEmpty(commonPageSizeName);

        if (canAddCommonPageSizePostfix)
            presenter += $" {commonPageSizeName}";

        return presenter;
    }
}
