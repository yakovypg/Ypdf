using Ypdf.CommandLine.Execution;

namespace Ypdf.CommandLine.Validation.Middlewares;

internal interface IValidationMiddleware
{
    ValidationResult Validate(IToolExecutionProvider executionProvider);
}
