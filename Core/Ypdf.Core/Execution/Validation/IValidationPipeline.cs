namespace Ypdf.Core.Execution.Validation;

public interface IValidationPipeline
{
    ValidationResult Run(IToolExecutionProvider executionProvider);
}
