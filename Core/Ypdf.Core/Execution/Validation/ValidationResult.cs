using System.Collections.Generic;

namespace Ypdf.Core.Execution.Validation;

public record ValidationResult(bool IsValid, IReadOnlyList<ValidationError> Errors)
{
    public static ValidationResult Success() => new(true, []);
    public static ValidationResult Fail(params ValidationError[] errors) => new(false, errors ?? []);
}
