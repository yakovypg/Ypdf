using System.Collections.Generic;

namespace Ypdf.Core.Tools;

public interface IMultipleInputTool : ITool
{
    void Execute(IEnumerable<string> inputPaths, string outputPath);
}
