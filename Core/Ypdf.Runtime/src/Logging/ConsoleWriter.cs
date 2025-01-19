using System;

namespace Ypdf.Runtime.Logging;

public class ConsoleWriter : OutputWriter
{
    public ConsoleWriter()
        : base(Console.Out) { }
}
