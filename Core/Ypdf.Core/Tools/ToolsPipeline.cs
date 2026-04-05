using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ypdf.Core.Config;
using Ypdf.Core.Execution;
using Ypdf.Core.FileSystem.Naming;

namespace Ypdf.Core.Tools;

public class ToolsPipeline : IMultipleInputTool
{
    private const string _tempFileExtension = "tmp";

    public ToolsPipeline(IEnumerable<ITool> tools)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(tools, nameof(tools));
        Tools = [.. tools];
    }

    protected IReadOnlyList<ITool> Tools { get; }

    public void Execute(string inputPath, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(inputPath, nameof(inputPath));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));
        DefaultExceptions.ThrowIfFileNotExists(inputPath, nameof(inputPath));

        Execute([inputPath], outputPath);
    }

    public void Execute(IEnumerable<string> inputPaths, string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(inputPaths, nameof(inputPaths));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));

        IList<IToolExecutionParameters> toolExecutionParameters = CreateToolExecutionParameters(inputPaths, outputPath);

        IList<IToolExecutionProvider> toolExecutionProviders = [.. toolExecutionParameters
            .Select(t => new ToolExecutionProvider(t))];

        for (int i = 0; i < toolExecutionProviders.Count; ++i)
        {
            IToolExecutionProvider toolExecutionProvider = toolExecutionProviders[i];
            toolExecutionProvider.ExecuteTool();

            if (i - 1 >= 0)
            {
                IToolExecutionProvider previousToolExecutionProvider = toolExecutionProviders[i - 1];
                File.Delete(previousToolExecutionProvider.ExecutionParameters.OutputPath);
            }
        }
    }

    protected IList<IToolExecutionParameters> CreateToolExecutionParameters(
        IEnumerable<string> inputPaths,
        string outputPath)
    {
        ExtendedArgumentException.ThrowIfNullOrEmpty(inputPaths, nameof(inputPaths));
        ExtendedArgumentException.ThrowIfNullOrWhiteSpace(outputPath, nameof(outputPath));

        var output = new List<IToolExecutionParameters>();
        string previousToolOutputPath = string.Empty;

        for (int i = 0; i < Tools.Count; ++i)
        {
            ITool tool = Tools[i];

            string toolOutputPath = i < Tools.Count - 1
                ? new UniqueFile(_tempFileExtension, CoreDirectories.TempDirectory).GetNext()
                : outputPath;

            ToolExecutionParameters toolExecutionParameters = i == 0
                ? new ToolExecutionParameters(tool, inputPaths, toolOutputPath)
                : new ToolExecutionParameters(tool, previousToolOutputPath, toolOutputPath);

            output.Add(toolExecutionParameters);
            previousToolOutputPath = toolOutputPath;
        }

        return output;
    }
}
