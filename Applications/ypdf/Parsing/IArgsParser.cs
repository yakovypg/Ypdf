using ExecutionLib.Configuration;

namespace ypdf.Parsing
{
    internal interface IArgsParser
    {
        Action<TextWriter> OptionDescriptionsWriter { get; }

        bool TryParse(string[] args, out YpdfConfig config);
        bool TryParse(string[] args, out YpdfConfig config, out Exception? exception);
    }
}