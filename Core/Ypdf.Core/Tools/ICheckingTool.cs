namespace Ypdf.Core.Tools;

public interface ICheckingTool : ITool
{
    bool Execute(string inputPath);
}
