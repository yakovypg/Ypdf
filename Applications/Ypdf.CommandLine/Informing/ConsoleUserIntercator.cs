using System;
using System.Collections.Generic;

namespace Ypdf.CommandLine.Informing;

internal sealed class ConsoleUserIntercator : UserInteractor
{
    internal ConsoleUserIntercator(IEnumerable<string>? yesAnswers = null)
        : base(Console.Out, Console.In, yesAnswers) { }
}
