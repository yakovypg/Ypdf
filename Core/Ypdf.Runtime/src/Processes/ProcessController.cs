using System;
using Ypdf.Runtime.Logging;

namespace Ypdf.Runtime.Processes;

public abstract class ProcessController : IProcessController
{
    protected ProcessController()
    {
        RedirectStandardError = true;
        RedirectStandardOutput = true;
        ThrowExceptionIfExitWithError = true;
    }

    public bool RedirectStandardError { get; set; }
    public bool RedirectStandardOutput { get; set; }
    public bool ThrowExceptionIfExitWithError { get; set; }

    public IOutputWriter? OutputWriter { get; set; }

    public Func<string?, string?>? ErrorDataConverter { get; set; }
    public Func<string?, string?>? OutputDataConverter { get; set; }
    public Predicate<string?>? ErrorDataVerifier { get; set; }
    public Predicate<string?>? OutputDataVerifier { get; set; }
}
