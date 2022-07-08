using Mono.Options;

namespace Ypdf
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool showHelp = false;
            double compressionRatio = 1;

            var filePaths = new List<string>();
            var extraArgs = new List<string>();

            var options = new OptionSet {
                { "f|file=", "the file path.", t => filePaths.Add(t) },
                { "c|compress=", "the compression ratio.", (double t) => compressionRatio = t },
                { "h|help", "show help.", t => showHelp = t is not null },
            };

            try
            {
                extraArgs = options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (showHelp)
            {
                ShowHelp(options);
                return;
            }

            string extraStr = string.Join(" ", extraArgs);
            Console.WriteLine($"extra {extraArgs.Count}: " + extraStr);

            string filesStr = string.Join(" ", filePaths);
            Console.WriteLine($"files {filePaths.Count}: " + filesStr);

            Console.WriteLine("compression: " + compressionRatio);
        }

        private static void ShowHelp(OptionSet options)
        {
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
        }
    }
}
