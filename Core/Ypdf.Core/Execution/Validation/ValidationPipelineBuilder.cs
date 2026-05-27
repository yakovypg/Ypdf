using System.Collections.Generic;
using Ypdf.Core.Execution.Validation.Middlewares;

namespace Ypdf.Core.Execution.Validation;

public class ValidationPipelineBuilder : IValidationPipelineBuilder
{
    private readonly List<IValidationMiddleware> _middlewares;
    private ValidationConfig _config;

    public ValidationPipelineBuilder()
    {
        _middlewares = [];
        _config = new();
    }

    public IValidationPipelineBuilder AddMiddleware(IValidationMiddleware middleware)
    {
        ExtendedArgumentNullException.ThrowIfNull(middleware, nameof(middleware));

        _middlewares.Add(middleware);
        return this;
    }

    public IValidationPipelineBuilder AddConfig(ValidationConfig config)
    {
        ExtendedArgumentNullException.ThrowIfNull(config, nameof(config));

        _config = config;
        return this;
    }

    public IValidationPipeline Build()
    {
        return new ValidationPipeline(_middlewares, _config);
    }
}
