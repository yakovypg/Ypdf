using System;
using System.Collections.Generic;
using NetArgumentParser.Subcommands;
using Ypdf.Core.Enumeration;

namespace Ypdf.CommandLine.Configuration.Restrictions;

internal sealed class AddWatermarkAnnotationSubcommandOptionRestrictions : OptionRestrictionProvider
{
    protected override IReadOnlyCollection<Action<Subcommand>> RestrictionProviders =>
    [
        AddRestrictionForInputPathOption,
        AddRestrictionForPagesOption,
        AddRestrictionForTextOption,
        AddRestrictionForWidthOption,
        AddRestrictionForHeightOption,
        AddRestrictionForLowerLeftPointOption,
        AddRestrictionForXTranslationOption,
        AddRestrictionForYTranslationOption,
        AddRestrictionForFontPathOption,
        AddRestrictionForFontSizeOption,
        AddRestrictionForFontOpacityOption
    ];

    private static void AddRestrictionForInputPathOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddFileAllowedRestrictionForPathOption(subcommand, AddWatermarkAnnotationSubcommand.InputPathLongName, PdfExtensions);
    }

    private static void AddRestrictionForPagesOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddGreaterThanZeroRestrictionForPagesOption<List<PageRange>>(subcommand, AddWatermarkAnnotationSubcommand.PagesLongName);
    }

    private static void AddRestrictionForTextOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddNotEmptyRestrictionForStringOption(subcommand, AddWatermarkAnnotationSubcommand.TextLongName);
    }

    private static void AddRestrictionForWidthOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForNumberOption<float>(subcommand, AddWatermarkAnnotationSubcommand.WidthLongName, 1, 100_000);
    }

    private static void AddRestrictionForHeightOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForNumberOption<float>(subcommand, AddWatermarkAnnotationSubcommand.HeightLongName, 1, 100_000);
    }

    private static void AddRestrictionForLowerLeftPointOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddCorrectRestrictionForFloatPointOption(subcommand, AddWatermarkAnnotationSubcommand.LowerLeftPointLongName, 0, 0);
    }

    private static void AddRestrictionForXTranslationOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddInRangeRestrictionForNumberOption<float>(
            subcommand,
            AddWatermarkAnnotationSubcommand.XTranslationLongName,
            -100_000,
            100_000);
    }

    private static void AddRestrictionForYTranslationOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));

        AddInRangeRestrictionForNumberOption<float>(
            subcommand,
            AddWatermarkAnnotationSubcommand.YTranslationLongName,
            -100_000,
            100_000);
    }

    private static void AddRestrictionForFontPathOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddFileAllowedRestrictionForPathOption(subcommand, AddWatermarkAnnotationSubcommand.FontPathLongName, FontExtensions);
    }

    private static void AddRestrictionForFontSizeOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForNumberOption<float>(subcommand, AddWatermarkAnnotationSubcommand.FontSizeLongName, 1, 512);
    }

    private static void AddRestrictionForFontOpacityOption(Subcommand subcommand)
    {
        ExtendedArgumentNullException.ThrowIfNull(subcommand, nameof(subcommand));
        AddInRangeRestrictionForNumberOption<float>(subcommand, AddWatermarkAnnotationSubcommand.FontOpacityLongName, 0, 1);
    }
}
