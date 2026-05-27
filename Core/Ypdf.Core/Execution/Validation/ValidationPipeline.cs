using System.Collections.Generic;
using Ypdf.Core.Execution.Validation.Middlewares;

namespace Ypdf.Core.Execution.Validation;

public class ValidationPipeline : IValidationPipeline
{
    private readonly IEnumerable<IValidationMiddleware> _middlewares;
    private readonly ValidationConfig _config;

    public ValidationPipeline(IEnumerable<IValidationMiddleware> middlewares, ValidationConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(middlewares, nameof(middlewares));
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        _middlewares = middlewares;
        _config = config;
    }

    public ValidationResult Run(IToolExecutionProvider executionProvider)
    {
        ExtendedArgumentNullException.ThrowIfNull(executionProvider, nameof(executionProvider));

        var errors = new List<ValidationError>();

        foreach (IValidationMiddleware middleware in _middlewares)
        {
            ValidationResult validationResult = middleware.Validate(executionProvider, _config);

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
