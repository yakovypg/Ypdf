using System.Collections.Generic;
using Ypdf.Core.Execution.Validation.Middlewares;

namespace Ypdf.Core.Execution.Validation;

public class ValidationPipelineBuilder : IValidationPipelineBuilder
{
    private readonly List<IValidationMiddleware> _middlewares = [];

    public IValidationPipelineBuilder Add(IValidationMiddleware middleware)
    {
        ExtendedArgumentNullException.ThrowIfNull(middleware, nameof(middleware));

        _middlewares.Add(middleware);
        return this;
    }

    public IValidationPipeline Build()
    {
        return new ValidationPipeline([.. _middlewares]);
    }
}
