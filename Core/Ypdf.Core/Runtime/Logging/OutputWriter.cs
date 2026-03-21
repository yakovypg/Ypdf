using System;
using System.IO;

namespace Ypdf.Core.Runtime.Logging;

public class OutputWriter : IOutputWriter
{
    public OutputWriter(TextWriter textWriter)
    {
        ExtendedArgumentNullException.ThrowIfNull(textWriter, nameof(textWriter));
        Writer = textWriter;
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
