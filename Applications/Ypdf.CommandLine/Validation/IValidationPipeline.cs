using Ypdf.CommandLine.Execution;

namespace Ypdf.CommandLine.Validation;

internal interface IValidationPipeline
{
    ValidationResult Run(IToolExecutionProvider executionProvider);
}
