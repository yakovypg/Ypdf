using System;
using System.IO;
using Ypdf.CommandLine.Exceptions;
using Ypdf.CommandLine.Execution;
using Ypdf.CommandLine.Informing;
using Ypdf.Core.Tools;

namespace Ypdf.CommandLine.Validation.Middlewares;

internal sealed class OutputDirectoryExistsMiddleware : IValidationMiddleware
{
    private readonly IUserInteractor _userInteractor;

    public OutputDirectoryExistsMiddleware(IUserInteractor userInteractor)
    {
        ExtendedArgumentNullException.ThrowIfNull(userInteractor, nameof(userInteractor));
        _userInteractor = userInteractor;
    }

    public ValidationResult Validate(IToolExecutionProvider executionProvider)
    {
        ExtendedArgumentNullException.ThrowIfNull(executionProvider, nameof(executionProvider));

        bool isSingleOutputCompressTool = executionProvider.Tool is CompressImageTool &&
            executionProvider.InputPaths.Count <= 1;

        bool isSingleOutputTool = executionProvider.Tool is not IMultipleOutputTool ||
            isSingleOutputCompressTool;

        string outputDirectoryPath = isSingleOutputTool
            ? GetFileDirectoryPath(executionProvider.OutputPath)
            : executionProvider.OutputPath;

        bool validationOk = Directory.Exists(outputDirectoryPath);

        if (validationOk)
            return ValidationResult.Success();

        string question = "Specified output directory doesn't exist. Create it? [Y/n] ";
        bool answer = _userInteractor.AskYesNo(question);

        if (!answer)
        {
            var error = new ValidationError(this, "Specified output directory doesn't exist.");
            return ValidationResult.Fail(error);
        }

        bool directoryCreated = TryCreateDirectory(outputDirectoryPath, out Exception? exception);

        if (!directoryCreated)
        {
            string reasonPostfix = exception is not null ? $": {exception.Message}" : ".";
            var error = new ValidationError(this, $"Failed to create output directory{reasonPostfix}");

            return ValidationResult.Fail(error);
        }

        return ValidationResult.Success();
    }

    private static string GetFileDirectoryPath(string path)
    {
        const string currentDirectory = ".";

        try
        {
            return Path.GetDirectoryName(path) ?? currentDirectory;
        }
        catch
        {
            return currentDirectory;
        }
    }

    private static bool TryCreateDirectory(string path, out Exception? exception)
    {
        try
        {
            Directory.CreateDirectory(path);
            exception = null;
            return true;
        }
        catch (Exception ex)
        {
            exception = ex;
            return false;
        }
    }
}
