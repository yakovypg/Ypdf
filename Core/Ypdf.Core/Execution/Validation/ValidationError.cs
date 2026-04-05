using Ypdf.Core.Execution.Validation.Middlewares;

namespace Ypdf.Core.Execution.Validation;

public record ValidationError(IValidationMiddleware Middleware, string Reason);
