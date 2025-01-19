using System;
using Ypdf.Runtime.Logging;

namespace Ypdf.Runtime.Processes;

public interface IProcessController
{
    bool RedirectStandardError { get; set; }
    bool RedirectStandardOutput { get; set; }
    bool ThrowExceptionIfExitWithError { get; set; }

    IOutputWriter? OutputWriter { get; set; }

    Func<string?, string?>? ErrorDataConverter { get; set; }
    Func<string?, string?>? OutputDataConverter { get; set; }
    Predicate<string?>? ErrorDataVerifier { get; set; }
    Predicate<string?>? OutputDataVerifier { get; set; }
}
