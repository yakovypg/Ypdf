using Ypdf.CommandLine.Validation.Middlewares;

namespace Ypdf.CommandLine.Validation;

internal interface IValidationPipelineBuilder
{
    IValidationPipelineBuilder Add(IValidationMiddleware middleware);
    IValidationPipeline Build();
}
