using ExecutionLib.Configuration;
using ExecutionLib.Execution;
using ExecutionLib.Informing.Logging;
using ExecutionLib.Informing.Users;
using System.Reflection;
using ypdf.Informing;
using ypdf.Parsing;

if (args.Length == 0)
{
    PrintVersion();
    return;
}

SharedConfig.Directories.Prepare();
SharedConfig.Files.Prepare();

var argsParser = new ArgsParser();

if (!argsParser.TryParse(args, out YpdfConfig config, out Exception? ex))
{
    Console.WriteLine(ex?.Message);
}
else if (config.ShowGuide)
{
    Guide.PrintSection("How to use CLI version");
}
else if (config.ShowHelp)
{
    Console.WriteLine("Options:");
    argsParser.OptionDescriptionsWriter.Invoke(Console.Out);
}
else if (config.ShowVersion)
{
    PrintVersion();
}
else
{
    var fileExistsQuestionFunc = FileExistsUserAnswerDispatcher.GetConsoleQuestionFunc();
    var fileExistsQuestion = new FileExistsUserAnswerDispatcher(fileExistsQuestionFunc);

    var applyCorrectionsQuestionFunc = ApplyCorrectionsUserAnswerDispatcher.GetConsoleQuestionFunc();
    var applyCorrectionsQuestion = new ApplyCorrectionsUserAnswerDispatcher(applyCorrectionsQuestionFunc);

    var logger = new ExecutionLogger(Console.Out);

    var executor = new PdfToolExecutor(config)
    {
        Logger = logger,
        FileExistsQuestion = fileExistsQuestion,
        ApplyCorrectionsQuestion = applyCorrectionsQuestion
    };

    IExecutionInfo executionInfo = executor.PrepareExecute();
    executor.Execute(executionInfo);
}

static void PrintVersion()
{
    AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
    string? name = assemblyName.Name?.ToString();
    string? version = assemblyName.Version?.ToString();
    Console.WriteLine($"{name} {version}");
}
