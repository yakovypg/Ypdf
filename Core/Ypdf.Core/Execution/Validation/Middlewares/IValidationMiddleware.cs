namespace Ypdf.Core.Execution.Validation.Middlewares;

public interface IValidationMiddleware
{
    ValidationResult Validate(IToolExecutionProvider executionProvider);
}
