using System;
using System.Collections.Generic;
using NetArgumentParser;
using NetArgumentParser.Informing;
using NetArgumentParser.Options;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Creators;
using Ypdf.CommandLine.Creators.Tools;
using Ypdf.CommandLine.Exceptions;
using Ypdf.CommandLine.Execution;
using Ypdf.CommandLine.Informing;
using Ypdf.CommandLine.Validation;
using Ypdf.CommandLine.Validation.Middlewares;

PrepareSystem();

GlobalConfig globalConfig = CreateGlobalConfig();
IUserInteractor userInteractor = new ConsoleUserIntercator();

try
{
    (ArgumentParser parser, YpdfParserConfig parserConfig) = CreateParser();
    SetRestrictions(parser);

    ParseArgumentsResult parseResult = parser.Parse(args);

    Dictionary<string, IToolCreator> supportedTools = GetSupportedTools(globalConfig);
    IValidationPipeline validationPipeline = CreateValidationPipeline(userInteractor);

    ToolExecutor toolExecutor = new(supportedTools, validationPipeline);
    toolExecutor.Execute(parserConfig, parseResult);
}
catch (Exception ex)
{
    new ErrorMessageWriter().WriteLine($"Error: {ex.Message}");
}

static void PrepareSystem()
{
    if (!Directories.TryPrepare())
        new ErrorMessageWriter().WriteLine("Cannot prepare system directories.");

    if (!FilePaths.TryPrepare())
        new ErrorMessageWriter().WriteLine("Cannot prepare system files.");
}

static GlobalConfig CreateGlobalConfig()
{
    if (!GlobalConfig.TryLoad(FilePaths.Config, out GlobalConfig globalConfig))
        new ErrorMessageWriter().WriteLine("Cannot load global config.");

    return globalConfig;
}

static (ArgumentParser Parser, YpdfParserConfig ParserConfig) CreateParser()
{
    YpdfParserConfig parserConfig = new();
    ArgumentParserCreator parserCreator = new();
    ArgumentParser parser = parserCreator.Create(parserConfig);

    return (parser, parserConfig);
}

static void SetRestrictions(ArgumentParser parser)
{
    ExtendedArgumentNullException.ThrowIfNull(parser, nameof(parser));

    SupportedOptionRestrictionProvidersCreator supportedOptionRestrictionProvidersCreator = new();

    Dictionary<string, IOptionConfigurationProvider> supportedOptionRestrictionProviders =
        supportedOptionRestrictionProvidersCreator.Create();

    OptionConfigurationSetter optionConfigurationSetter = new(supportedOptionRestrictionProviders);
    optionConfigurationSetter.SetOptionConfigurations(parser);
}

static Dictionary<string, IToolCreator> GetSupportedTools(GlobalConfig globalConfig)
{
    ExtendedArgumentNullException.ThrowIfNull(globalConfig, nameof(globalConfig));

    SupportedToolsCreator supportedToolsCreator = new();
    return supportedToolsCreator.Create(globalConfig);
}

static IValidationPipeline CreateValidationPipeline(IUserInteractor userInteractor)
{
    ExtendedArgumentNullException.ThrowIfNull(userInteractor, nameof(userInteractor));

    var validationPipelineBuilder = new ValidationPipelineBuilder()
        .Add(new OutputDirectoryExistsMiddleware(userInteractor))
        .Add(new OutputFileNotExistsMiddleware(userInteractor));

    return validationPipelineBuilder.Build();
}
