using System.IO;
using Ypdf.CommandLine.Exceptions;
using Ypdf.CommandLine.Informing;
using Ypdf.CommandLine.Tools;
using Ypdf.Core.Execution;
using Ypdf.Core.Execution.Validation;
using Ypdf.Core.Execution.Validation.Middlewares;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Validation.Middlewares;

internal sealed class OutputFileNotExistsMiddleware : IValidationMiddleware
{
    private readonly IUserInteractor _userInteractor;

    public OutputFileNotExistsMiddleware(IUserInteractor userInteractor)
    {
        ExtendedArgumentNullException.ThrowIfNull(userInteractor, nameof(userInteractor));
        _userInteractor = userInteractor;
    }

    public ValidationResult Validate(IToolExecutionProvider executionProvider)
    {
        ExtendedArgumentNullException.ThrowIfNull(executionProvider, nameof(executionProvider));

        bool isSingleOutputCompressTool = executionProvider.ExecutionParameters.Tool is CompressImageTool &&
            executionProvider.ExecutionParameters.InputPaths.Count <= 1;

        bool isSingleOutputTool = executionProvider.ExecutionParameters.Tool is not IMultipleOutputTool ||
            isSingleOutputCompressTool;

        bool isConfigTool = executionProvider.ExecutionParameters.Tool is ShowGlobalConfigTool ||
            executionProvider.ExecutionParameters.Tool is ResetGlobalConfigTool;

        bool validationOk =
            isConfigTool ||
            !isSingleOutputTool ||
            !File.Exists(executionProvider.ExecutionParameters.OutputPath);

        if (validationOk)
            return ValidationResult.Success();

        string question = "Specified output file already exist. Overwrite it? [Y/n] ";
        bool answer = _userInteractor.AskYesNo(question);

        if (!answer)
        {
            var error = new ValidationError(this, "Specified output file already exist.");
            return ValidationResult.Fail(error);
        }

        return ValidationResult.Success();
    }
}
