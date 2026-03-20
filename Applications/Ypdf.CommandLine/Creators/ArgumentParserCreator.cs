using System;
using System.Collections.Generic;
using System.Reflection;
using iText.Kernel.Colors;
using NetArgumentParser;
using NetArgumentParser.Converters;
using NetArgumentParser.Generators;
using Ypdf.CommandLine.Converters;
using Ypdf.Core.Design;
using Ypdf.Core.Design.Pages;
using Ypdf.Core.Enumeration;
using Ypdf.Core.Geometry;
using Ypdf.Core.Security;

namespace Ypdf.CommandLine.Creators;

internal sealed class ArgumentParserCreator : IArgumentParserCreator
{
    public ArgumentParser Create(object parserConfig)
    {
        ExtendedArgumentNullException.ThrowIfNull(parserConfig, nameof(parserConfig));

        string? programVersion = Assembly
            .GetEntryAssembly()?
            .GetName()
            .Version?
            .ToString();

        var parser = new ArgumentParser()
        {
            NumberOfArgumentsToSkip = 0,
            RecognizeCompoundOptions = true,
            RecognizeSlashOptions = false,
            UseDefaultHelpOption = true,
            UseDefaultVersionOption = true,
            ProgramName = "ypdf",
            ProgramVersion = $"ypdf v{programVersion}",
            ProgramDescription = "PDF document processing application",

            SubcommandDescriptionGeneratorCreator =
                t => new SubcommandDescriptionGenerator(t)
        };

        ConfigureParser(parser, parserConfig);

        return parser;
    }

    private static void ConfigureParser(ArgumentParser parser, object config)
    {
        ExtendedArgumentNullException.ThrowIfNull(parser, nameof(parser));
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        var parserGenerator = new ArgumentParserGenerator();
        parserGenerator.ConfigureParser(parser, config);

        ConsoleTextWriter outputWriter = new();
        IEnumerable<IValueConverter> converters = CreateConverters();
        ApplicationDescriptionGenerator descriptionGenerator = CreateDescriptionGenerator(parser);

        parser.DescriptionGenerator = descriptionGenerator;

        parser.ChangeOutputWriter(outputWriter);
        parser.AddConverters(true, [..converters]);
    }

    private static ApplicationDescriptionGenerator CreateDescriptionGenerator(
        ArgumentParser parser)
    {
        ExtendedArgumentNullException.ThrowIfNull(parser, nameof(parser));

        return new ApplicationDescriptionGenerator(parser)
        {
            UsageHeader = "Usage: ",
            OptionExamplePrefix = new string(' ', 2),
            DelimiterAfterOptionExample = new string(' ', 2),
            SubcommandsHeader = "Subcommands:",
            SubcommandNamePrefix = new string(' ', 2),
            DelimiterAfterSubcommandName = new string(' ', 2),
            OptionExampleCharsLimit = 30,
            WindowWidth = Console.WindowWidth
        };
    }

    private static IEnumerable<IValueConverter> CreateConverters()
    {
        return [
            new ValueConverter<Margin>(Margin.Parse),
            new ValueConverter<PageNumberTextPresenter>(PageNumberTextPresenter.Parse),
            new MultipleValueConverter<PageContentShift>(PageContentShift.ParseMany),
            new ValueConverter<Color>(ColorConverter.Parse),
            new MultipleValueConverter<PageRange>(PageRange.Parse),
            new ValueConverter<FloatPoint>(FloatPoint.Parse),
            new MultipleValueConverter<PageCropping>(PageCropping.ParseMany),
            new MultipleValueConverter<PageDivision>(PageDivision.ParseMany),
            new ValueConverter<PageOrder>(PageOrder.Parse),
            new MultipleValueConverter<PageRotation>(PageRotation.ParseMany),
            new ValueConverter<EncryptionAlgorithm>(EncryptionAlgorithm.Parse),
            new ValueConverter<MathExpression>(MathExpression.Parse)
        ];
    }
}
