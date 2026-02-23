using System;
using System.Collections.Generic;
using NetArgumentParser;
using NetArgumentParser.Informing;
using Ypdf.CommandLine.Configuration;
using Ypdf.CommandLine.Configuration.Restrictions;
using Ypdf.CommandLine.Creators;
using Ypdf.CommandLine.Creators.Tools;
using Ypdf.CommandLine.Execution;
using Ypdf.CommandLine.Informing;
using Ypdf.Paths;

var errorMessageWriter = new ErrorMessageWriter();

if (!PathManager.TryPrepareDirectories())
    errorMessageWriter.WriteLine("Cannot prepare system directories.");

if (!PathManager.TryPrepareFiles())
    errorMessageWriter.WriteLine("Cannot prepare system files.");

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
    Dictionary<string, IToolCreator> supportedTools = supportedToolsCreator.Create();
    ParseArgumentsResult parseResult = parser.Parse(args);

    ToolExecutor toolExecutor = new(supportedTools);
    toolExecutor.Execute(parserConfig, parseResult);
}
catch (Exception ex)
{
    errorMessageWriter.WriteLine(ex.Message);
}
