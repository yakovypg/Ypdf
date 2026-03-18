using System;

namespace Ypdf.Core.Runtime.Logging;

public class ConsoleWriter : OutputWriter
{
    public ConsoleWriter()
        : base(Console.Out) { }
}
