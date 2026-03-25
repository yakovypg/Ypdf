namespace Ypdf.Core.Runtime.Processes;

public record struct ProcessResult(string FileName, int ExitCode)
{
    public readonly bool IsSuccessful => ExitCode == 0;
}
