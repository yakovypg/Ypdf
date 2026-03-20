using Ypdf.CommandLine.Validation.Middlewares;

namespace Ypdf.CommandLine.Validation;

internal sealed record ValidationError(IValidationMiddleware Middleware, string Reason);
