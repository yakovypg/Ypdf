using System;
using System.Collections.Generic;
using NetArgumentParser;
using NetArgumentParser.Informing;
using Ypdf.CommandLine.AppConfig;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Configuration.Restrictions;
using Ypdf.CommandLine.Creators;
using Ypdf.CommandLine.Creators.Tools;
using Ypdf.CommandLine.Execution;
using Ypdf.CommandLine.Informing;

var errorMessageWriter = new ErrorMessageWriter();

if (!Directories.TryPrepare())
    errorMessageWriter.WriteLine("Cannot prepare system directories.");

if (!FilePaths.TryPrepare())
    errorMessageWriter.WriteLine("Cannot prepare system files.");

if (!GlobalConfig.TryLoad(FilePaths.Config, out GlobalConfig globalConfig))
    errorMessageWriter.WriteLine("Cannot load global config.");

YpdfParserConfig parserConfig = new();
ArgumentParserCreator parserCreator = new();
ArgumentParser parser = parserCreator.Create(parserConfig);

SupportedOptionRestrictionProvidersCreator supportedOptionRestrictionProvidersCreator = new();

Dictionary<string, IOptionRestrictionProvider> supportedOptionRestrictionProviders =
    supportedOptionRestrictionProvidersCreator.Create();

OptionRestrictionSetter optionRestrictionsSetter = new(supportedOptionRestrictionProviders);
optionRestrictionsSetter.SetRestrictions(parser);

try
{
    SupportedToolsCreator supportedToolsCreator = new();
    Dictionary<string, IToolCreator> supportedTools = supportedToolsCreator.Create(globalConfig);
    ParseArgumentsResult parseResult = parser.Parse(args);

    ToolExecutor toolExecutor = new(supportedTools);
    toolExecutor.Execute(parserConfig, parseResult);
}
catch (Exception ex)
{
    errorMessageWriter.WriteLine(ex.Message);
}
