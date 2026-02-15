using NetArgumentParser;

namespace Ypdf.CommandLine.Creators;

internal interface IArgumentParserCreator
{
    ArgumentParser Create(object config);
}
