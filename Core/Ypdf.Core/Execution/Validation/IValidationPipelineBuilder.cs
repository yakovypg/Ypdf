using Ypdf.Core.Execution.Validation.Middlewares;

namespace Ypdf.Core.Execution.Validation;

public interface IValidationPipelineBuilder
{
    IValidationPipelineBuilder AddMiddleware(IValidationMiddleware middleware);
    IValidationPipelineBuilder AddConfig(ValidationConfig config);
    IValidationPipeline Build();
}
