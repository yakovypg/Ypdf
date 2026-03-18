using System;
using System.IO;

namespace Ypdf.Core.Runtime.Logging;

public class OutputWriter : IOutputWriter
{
    public OutputWriter(TextWriter textWriter)
    {
        Writer = textWriter
            ?? throw new ArgumentNullException(nameof(textWriter));
    }

    protected TextWriter Writer { get; }

    public virtual void Write(string? text)
    {
        Writer.Write(text);
    }

    public virtual void WriteLine(string? text = null)
    {
        Writer.WriteLine(text ?? string.Empty);
    }
}
