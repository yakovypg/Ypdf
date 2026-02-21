using System;
using System.Collections.Generic;
using NetArgumentParser;
using NetArgumentParser.Informing;
using Ypdf.CommandLine.Configuration;
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

var parserConfig = new YpdfParserConfig();
var parserCreator = new ArgumentParserCreator();
var supportedToolsCreator = new SupportedToolsCreator();

ArgumentParser parser = parserCreator.Create(parserConfig);

try
{
    Dictionary<string, IToolCreator> supportedTools = supportedToolsCreator.Create();
    ParseArgumentsResult parseResult = parser.Parse(args);

    var toolExecutor = new ToolExecutor(supportedTools);
    toolExecutor.Execute(parserConfig, parseResult);
}
catch (Exception ex)
{
    errorMessageWriter.WriteLine(ex.Message);
}
