using System.Collections.Generic;
using Ypdf.CommandLine.Execution;
using Ypdf.CommandLine.Validation.Middlewares;

namespace Ypdf.CommandLine.Validation;

internal sealed class ValidationPipeline : IValidationPipeline
{
    private readonly IValidationMiddleware[] _middlewares;
    public ValidationPipeline(IValidationMiddleware[] middlewares) => _middlewares = middlewares;

    public ValidationResult Run(IToolExecutionProvider executionProvider)
    {
        ExtendedArgumentNullException.ThrowIfNull(executionProvider, nameof(executionProvider));

        var errors = new List<ValidationError>();

        foreach (IValidationMiddleware middleware in _middlewares)
        {
            ValidationResult validationResult = middleware.Validate(executionProvider);

            if (!validationResult.IsValid)
            {
                errors.AddRange(validationResult.Errors);
                return ValidationResult.Fail([.. errors]);
            }
        }

        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Fail([.. errors]);
    }
}
