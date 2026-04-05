using Ypdf.Core.Execution.Validation.Middlewares;

namespace Ypdf.Core.Execution.Validation;

public interface IValidationPipelineBuilder
{
    IValidationPipelineBuilder Add(IValidationMiddleware middleware);
    IValidationPipeline Build();
}
