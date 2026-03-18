namespace Ypdf.Core.Runtime.Logging;

public interface IOutputWriter
{
    void Write(string? text);
    void WriteLine(string? text = null);
}
