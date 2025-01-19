using System;
using System.IO;

namespace Ypdf.Runtime.Logging;

public class OutputWriter : IOutputWriter
{
    public OutputWriter(TextWriter textWriter)
    {
        Writer = textWriter
            ?? throw new ArgumentNullException(nameof(textWriter));
    }

    protected TextWriter Writer { get; }

    public void Write(string? text)
    {
        Writer.Write(text);
    }

    public void WriteLine(string? text = null)
    {
        Writer.WriteLine(text ?? string.Empty);
    }
}
